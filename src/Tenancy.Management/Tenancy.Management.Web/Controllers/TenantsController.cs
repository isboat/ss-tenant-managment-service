﻿using Microsoft.AspNetCore.Http;
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
            //var list = await _tenantService.GetTenantsAsync();
            var list = new List<TenantModel> { new TenantModel { Name = "Name one"}, new TenantModel { Name = "Name two"} };
            return View(list);
        }

        // GET: TenantController/Details/5
        public ActionResult Details(int id)
        {
            return View(new TenantModel {  Name = "Tenant Name"});
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TenantController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: TenantController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TenantController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
