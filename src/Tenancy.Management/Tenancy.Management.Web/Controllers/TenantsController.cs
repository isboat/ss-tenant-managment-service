using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using Tenancy.Management.Models;
using Tenancy.Management.Services;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Models;

namespace Tenancy.Management.Web.Controllers
{
    public class TenantsController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IUserService _userService;
        private readonly ITenantModelService<AssetModel> _assetService;
        private readonly ITenantModelService<MenuModel> _menuService;
        private readonly ITenantModelService<TextAssetItemModel> _textAssetService;
        private readonly IEmailSender _emailSender;

        public TenantsController(
            ITenantService tenantService,
            IUserService userService,
            ITenantModelService<AssetModel> assetService,
            ITenantModelService<MenuModel> menuService,
            ITenantModelService<TextAssetItemModel> textAssetService,
            IEmailSender emailSender)
        {
            _tenantService = tenantService;
            _userService = userService;
            _assetService = assetService;
            _menuService = menuService;
            _textAssetService = textAssetService;
            _emailSender = emailSender;
        }

        // GET: TenantController
        public async Task<ActionResult> Index()
        {
            var model = new TenantListViewModel {  Tenants = new List<TenantViewModel>()};
            var list = await _tenantService.GetTenantsAsync();
            foreach (var tenant in list)
            {
                var users = await _userService.GetUsersAsync(tenant.Id!);
                var viewModel = new TenantViewModel
                {
                    Tenant = tenant,
                    Users = users
                };

                model.Tenants.Add(viewModel);

            }

            return View(model);
        }

        public async Task<ActionResult> Details(string id)
        {
            var tenant = await _tenantService.GetTenantAsync(id);
            var users = await _userService.GetUsersAsync(tenant.Id!);
            var assets = await _assetService.GetAllAsync(tenant.Id!);
            var menus = await _menuService.GetAllAsync(tenant.Id!);
            var textAds = await _textAssetService.GetAllAsync(tenant.Id!);

            var model = new TenantViewModel();
            if (tenant != null) model.Tenant = tenant;
            if (users != null) model.Users = users;
            if (assets != null) model.Assets = assets;
            if (assets != null) model.Menus = menus;
            if (textAds != null) model.TextAssets = textAds;

            return View(model);
        }

        // GET: TenantController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TenantController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm]TenantModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //model.Id = Guid.NewGuid().ToString("N");
                    model.Created = DateTime.Now;

                    await _tenantService.CreateAsync(model);

                    await _emailSender.SendEmailAsync(model.Email!, "onScreenSync platform created", GetTenantCreatedEmailBody(model));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private static string GetTenantCreatedEmailBody(TenantModel model)
        {
            var builder = new StringBuilder();
            builder.Append($"<p>Dear {model.Name},</p>");
            builder.Append($"<p>Welcome to onScreenSync TV Screen Management service! Your platform is now created and opened for use. We're thrilled to have you on board and we're excited to see what you'll accomplish with our platform. Here are a few things you can do now that you're part of our community:</p>");
            builder.Append("<ul>");
            builder.Append($"<li><strong>Explore Our Features</strong>: Take some time to navigate through our platform and discover all the tools and features we offer to help you. Visit <a href='http://myscreensyncservice.runasp.net/'>Management Dashboard to get started.</a></li>");
            builder.Append("<li><strong>Add users as content editors</strong>: From the management dashboard, you can add users who can add and updated engaging content for your audience.</li>");
            builder.Append("</ul>");
            builder.Append("<p>If you have any questions or need assistance, don't hesitate to reach out to our support team at support@onscreensync.com or visit our Help Center for <a href='https://onscreensync.com/faq.html'>FAQs and troubleshooting guides</a>.</p>");
            builder.Append("<p>Once again, welcome to onScreenSync TV Screen Management service! We're committed to providing you with a seamless and rewarding experience, and we look forward to helping you achieve your goals.</p>");
            builder.Append("<p>Best regards,<br />onScreenSync.com<p/>");
            return builder.ToString();
        }

        // GET: TenantController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _tenantService.GetTenantAsync(id);
            return View(model);
        }

        // POST: TenantController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [FromForm] TenantModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _tenantService.UpdateAsync(id, model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("/Tenants/Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                //await _tenantService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
