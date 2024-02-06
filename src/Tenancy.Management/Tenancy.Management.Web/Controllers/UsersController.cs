using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Tenancy.Management.Models;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{tenantId}/Users")]
        public async Task<ActionResult> Index(string tenantId)
        {
            var list = await _userService.GetUsersAsync(tenantId);
            return View(list);
        }

        [HttpGet("{tenantId}/Users/Details/{id}")]
        public async Task<ActionResult> Details(string tenantId, string id)
        {
            var model = await _userService.GetAsync(id);
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

                    await _userService.CreateAsync(model);
                }

                return RedirectToAction(nameof(Index), new { tenantId = tenantId});
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("{tenantId}/Users/Edit/{id}")]
        public async Task<ActionResult> Edit(string tenantId, string id)
        {
            var model = await _userService.GetAsync(id);
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
                    await _userService.UpdateAsync(id, model);
                }
                return RedirectToAction(nameof(Index), new { tenantId = tenantId });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("{tenantId}/Users/Delete/{id}")]
        public async Task<ActionResult> Delete(string tenantId, string id)
        {
            try
            {
                await _userService.RemoveAsync(id);
                return RedirectToAction(nameof(Index), new { tenantId });
            }
            catch
            {
                return RedirectToAction(nameof(Index), new { tenantId });
            }
        }
    }
}
