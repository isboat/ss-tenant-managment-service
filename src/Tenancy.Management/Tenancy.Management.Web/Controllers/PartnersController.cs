using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Tenancy.Management.Models;
using Tenancy.Management.Services;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Models;

namespace Tenancy.Management.Web.Controllers
{
    [Authorize]
    public class PartnersController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly ITenantModelService<AssetModel> _assetService;
        private readonly ITenantModelService<MenuModel> _menuService;
        private readonly ITenantModelService<TextAssetItemModel> _textAssetService;
        private readonly IService<PartnerModel> _partnerService;
        private readonly IEmailSender _emailSender;
        private readonly AuthSettings _authSettings;
        private readonly ILogger<PartnersController> _logger;

        public PartnersController(
            ITenantService tenantService,
            IUserService userService,
            ITenantModelService<AssetModel> assetService,
            ITenantModelService<MenuModel> menuService,
            ITenantModelService<TextAssetItemModel> textAssetService,
            IService<PartnerModel> partnerService,
            IEncryptionService encryptionService,
            IEmailSender emailSender,
            Microsoft.Extensions.Options.IOptions<AuthSettings> authSettings,
            ILogger<PartnersController> logger)
        {
            _tenantService = tenantService;
            _userService = userService;
            _assetService = assetService;
            _menuService = menuService;
            _textAssetService = textAssetService;
            _emailSender = emailSender;
            _partnerService = partnerService;
            _encryptionService = encryptionService;
            _authSettings = authSettings.Value;
            _logger = logger;
        }

        // GET: PartnersController
        public async Task<ActionResult> Index()
        {
            var partners = await _partnerService.GetAllAsync();

            return View(partners);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return View(nameof(Index));

            var model = await _partnerService.GetAsync(id);
            if(model == null) return View(nameof(Index));

            return View(model);
        }

        [HttpGet("/Partners/{id}/tenants")]
        public async Task<ActionResult> Tenants(string id)
        {
            if (string.IsNullOrEmpty(id)) return View(nameof(Index));

            var model = await _tenantService.GetByFilter(x => x.PartnerId == id);
            if (model == null) return View(nameof(Index));

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
        public async Task<ActionResult> Create([FromForm]PartnerModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = Guid.NewGuid().ToString("N");
                    model.Created = DateTime.Now;
                    var inviteToken = _encryptionService.GenerateToken();
                    model.Password = null;
                    model.InviteTokenHash = _encryptionService.HashToken(inviteToken);
                    model.InviteTokenExpiresOn = DateTime.UtcNow.AddHours(_authSettings.InviteTokenExpiryHours);
                    model.InviteTokenConsumedOn = null;

                    await _partnerService.CreateAsync(model);

                    await _emailSender.SendEmailAsync(model.Email!, "onScreenSync partnership platform created", EmailTemplates.GetPartnerInviteEmailBody(model, inviteToken));
                    await _emailSender.SendEmailAsync(model.Email!, "General Data Protection Regulation (UK GDPR)", EmailTemplates.GetTenantGdprEmailBody(model.Name));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create partner");
                return View();
            }
        }

        // GET: TenantController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _partnerService.GetAsync(id);
            return View(model);
        }

        // POST: TenantController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [FromForm] PartnerModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _partnerService.UpdateAsync(id, model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update partner {PartnerId}", id);
                return View();
            }
        }

        [HttpPost("/Partners/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _partnerService.RemoveAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete partner {PartnerId}", id);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
