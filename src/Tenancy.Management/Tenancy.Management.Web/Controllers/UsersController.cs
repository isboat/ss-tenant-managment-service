using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Tenancy.Management.Models;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Models;

namespace Tenancy.Management.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IEmailSender emailSender, ILogger<UsersController> logger)
        {
            _userService = userService;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet("{tenantId}/Users")]
        public async Task<ActionResult> Index(string tenantId)
        {
            var list = await _userService.GetUsersAsync(tenantId);
            var model = new UserListViewModel
            {
                Users = list ?? new List<UserModel>(),
                TenantId = tenantId
            };
            return View(model);
        }

        [HttpGet("{tenantId}/Users/Details/{id}")]
        public async Task<ActionResult> Details(string tenantId, string id)
        {
            var model = await _userService.GetAsync(tenantId, id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpGet("{tenantId}/Users/Create")]
        public ActionResult Create(string tenantId)
        {
            var model = new UserModel { TenantId = tenantId };
            return View(model);
        }

        [HttpPost("{tenantId}/Users/Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string tenantId, [FromForm]UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = Guid.NewGuid().ToString("N");
                    model.CreatedOn = DateTime.UtcNow;

                    var existingUser = await _userService.GetByEmailAsync(model.Email!);
                    if (existingUser != null)
                    {
                        return RedirectToAction(nameof(Create), new { tenantId });
                    }

                    var inviteToken = await _userService.CreateAsync(model);
                    await _emailSender.SendEmailAsync(model.Email!, "onScreenSync user created", EmailTemplates.GetUserInviteEmailBody(model, inviteToken));
                }

                return RedirectToAction(nameof(Index), new { tenantId = tenantId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user for tenant {TenantId}", tenantId);
                return View();
            }
        }

        [HttpGet("{tenantId}/Users/Edit/{id}")]
        public async Task<ActionResult> Edit(string tenantId, string id)
        {
            var model = await _userService.GetAsync(tenantId, id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost("{tenantId}/Users/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string tenantId, string id, [FromForm] UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await _userService.GetAsync(tenantId, id);
                    if (existingUser == null) return NotFound();

                    model.TenantId = tenantId;
                    await _userService.UpdateAsync(id, model);
                }
                return RedirectToAction(nameof(Index), new { tenantId = tenantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user {UserId} for tenant {TenantId}", id, tenantId);
                return View();
            }
        }

        [HttpPost("{tenantId}/Users/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string tenantId, string id)
        {
            try
            {
                await _userService.RemoveAsync(tenantId, id);
                return RedirectToAction(nameof(Index), new { tenantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user {UserId} for tenant {TenantId}", id, tenantId);
                return RedirectToAction(nameof(Index), new { tenantId });
            }
        }
    }
}
