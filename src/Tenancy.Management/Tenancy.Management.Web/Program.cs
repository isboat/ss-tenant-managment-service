using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tenancy.Management.Models;
using Tenancy.Management.Models.Signalr;
using Tenancy.Management.Mongo;
using Tenancy.Management.Mongo.Interfaces;
using Tenancy.Management.Services;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Services;

namespace Tenancy.Management.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            builder.Services.Configure<AuthSettings>(
                builder.Configuration.GetSection("AuthSettings"));


            ConfigureServices(builder);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            RegisterAuth(builder, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ITenantDBRepository<TenantModel>, TenantAdminRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IRepository<RegisterModel>, RegistrationRepository>();

            builder.Services.AddSingleton<ITenantService, TenantService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();

            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IRegistrationService, RegistrationService>();
            builder.Services.AddSingleton<IService<PartnerModel>, BaseService<PartnerModel>>();


            builder.Services.AddSingleton<ITenantRepository<AssetModel>>(provider =>
            {
                var settings = provider.GetService<IOptions<MongoSettings>>();
                return new TenantModelRepository<AssetModel>(settings!, "AssetItems");
            });

            builder.Services.AddSingleton<ITenantRepository<MenuModel>>(provider =>
            {
                var settings = provider.GetService<IOptions<MongoSettings>>();
                return new TenantModelRepository<MenuModel>(settings!, "Menus");
            });

            builder.Services.AddSingleton<ITenantRepository<TextAssetItemModel>>(provider =>
            {
                var settings = provider.GetService<IOptions<MongoSettings>>();
                return new TenantModelRepository<TextAssetItemModel>(settings!, "TextAssetItems");
            });

            builder.Services.AddSingleton<ITenantRepository<SignalrConnectionModel>>(provider =>
            {
                var settings = provider.GetService<IOptions<MongoSettings>>();
                return new TenantModelRepository<SignalrConnectionModel>(settings!, "SignalrConnections");
            });

            builder.Services.AddScoped<ISignalrService>(provider =>
            {
                var serviceBusConnectionString = builder.Configuration.GetValue<string>(SignalRConstants.AzureSignalRConnectionStringName);
                return new SignalrService(serviceBusConnectionString!, ServiceTransportType.Transient);
            });

            builder.Services.AddSingleton<IRepository<DeviceAuthModel>, DeviceRepository>();
            builder.Services.AddSingleton<IRepository<PartnerModel>, PartnerRepository>();
            builder.Services.AddSingleton<IService<DeviceAuthModel>, BaseService<DeviceAuthModel>>();
            builder.Services.AddSingleton<ITenantModelService<AssetModel>, TenantModelService<AssetModel>>();
            builder.Services.AddSingleton<ITenantModelService<TextAssetItemModel>, TenantModelService<TextAssetItemModel>>();
            builder.Services.AddSingleton<ITenantModelService<MenuModel>, TenantModelService<MenuModel>>();
            builder.Services.AddSingleton<ITenantModelService<SignalrConnectionModel>, TenantModelService<SignalrConnectionModel>>();
        }

        private static void RegisterAuth(WebApplicationBuilder builder, ConfigurationManager configuration)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(TenantAuthorization.RequiredPolicy, policy =>
                    policy.RequireAuthenticatedUser().RequireClaim("scope", TenantAuthorization.RequiredScope));
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Login";
                    options.Cookie.Name = "onScreenSync.Tenancy.AspNetCore.Cookies";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.SlidingExpiration = true;

                });
        }
    }
}