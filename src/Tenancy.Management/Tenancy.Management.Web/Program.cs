using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Options;
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

            ConfigureServices(builder);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
            builder.Services.AddSingleton<ITenantService, TenantService>();
            builder.Services.AddSingleton<IUserService, UserService>();


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
            builder.Services.AddSingleton<IService<DeviceAuthModel>, BaseService<DeviceAuthModel>>();
            builder.Services.AddSingleton<ITenantModelService<AssetModel>, TenantModelService<AssetModel>>();
            builder.Services.AddSingleton<ITenantModelService<TextAssetItemModel>, TenantModelService<TextAssetItemModel>>();
            builder.Services.AddSingleton<ITenantModelService<MenuModel>, TenantModelService<MenuModel>>();
            builder.Services.AddSingleton<ITenantModelService<SignalrConnectionModel>, TenantModelService<SignalrConnectionModel>>();
        }
    }
}