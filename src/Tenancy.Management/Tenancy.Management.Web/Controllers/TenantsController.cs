using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Tenancy.Management.Models;
using Tenancy.Management.Services.Interfaces;

namespace Tenancy.Management.Web.Controllers
{
    public class TenantsController : Controller
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // GET: TenantController
        public async Task<ActionResult> Index()
        {
            var list = await _tenantService.GetTenantsAsync();
            return View(list);
        }

        // GET: TenantController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model = await _tenantService.GetTenantAsync(id);
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
                    model.Id = Guid.NewGuid().ToString("N");
                    model.Created = DateTime.Now;

                    await _tenantService.CreateAsync(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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

        // GET: TenantController/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: TenantController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
