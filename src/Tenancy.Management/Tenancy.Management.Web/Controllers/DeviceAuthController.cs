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
    public class DeviceAuthController : Controller
    {
        private readonly IService<DeviceAuthModel> _baseService;

        public DeviceAuthController(IService<DeviceAuthModel> baseService)
        {
            _baseService = baseService;
        }

        // GET: TenantController
        public async Task<ActionResult> Index()
        {
            var list = await _baseService.GetByFilterAsync((x) => FilterAsync(x));

            return View(list);
        }

        [HttpGet("/DeviceAuth/Delete")]
        public async Task<ActionResult> Delete()
        {
            try
            {
                var list = await _baseService.GetByFilterAsync((x) => FilterAsync(x));
                foreach (var item in list)
                {
                    await _baseService.RemoveAsync(item.Id!);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private bool FilterAsync(DeviceAuthModel model)
        {
            if (IsApproved(model)) return false;

            return IsExpired(model);
        }

        private bool IsExpired(DeviceAuthModel model)
        {
            if (model?.RegisteredDatetime == null || model?.ExpiresIn == null) return true;

            var expirationDatetime = model.RegisteredDatetime.Value.AddSeconds(model.ExpiresIn.Value);
            return expirationDatetime < DateTime.UtcNow;
        }

        private bool IsApproved(DeviceAuthModel model)
        {
            return model?.ApprovedDatetime != null
                && model.ApprovedDatetime > DateTime.UnixEpoch
                && model.ApprovedDatetime > model.RegisteredDatetime;

        }
    }
}
