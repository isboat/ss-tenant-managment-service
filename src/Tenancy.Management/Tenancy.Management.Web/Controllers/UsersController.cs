using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
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

                    await _userService.CreateAsync(model);
                    await _emailSender.SendEmailAsync(model.Email!, "onScreenSync user created", GetUserCreatedEmailbody(model));
                }

                return RedirectToAction(nameof(Index), new { tenantId = tenantId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user for tenant {TenantId}", tenantId);
                return View();
            }
        }

        private static string GetUserCreatedEmailbody(UserModel model)
        {
            var builder = new StringBuilder();
            builder.Append($"<p>Dear {model.Name},</p>");
            builder.Append($"<p>Welcome to onScreenSync TV Screen Management service! Welcome onboard as a content editor</p>");
            builder.Append("<ul>");
            builder.Append($"<li>Take some time to navigate through our platform and discover all the tools and features we offer to help you. Visit <a href='https://dashboard.onscreensync.com'>Management Dashboard</a> to get started</li>");
            builder.Append("</ul>");
            builder.Append("<p>If you have any questions or need assistance, don't hesitate to reach out to our support team at support@onscreensync.com or visit our Help Center for <a href='https://onscreensync.com/faq.html'>FAQs and troubleshooting guides</a>.</p>");
            builder.Append("<p>Best regards,<br />onScreenSync.com<p/>");
            return builder.ToString();
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
