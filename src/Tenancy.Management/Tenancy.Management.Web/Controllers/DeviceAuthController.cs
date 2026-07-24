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
        private readonly ILogger<DeviceAuthController> _logger;

        public DeviceAuthController(IService<DeviceAuthModel> baseService, ILogger<DeviceAuthController> logger)
        {
            _baseService = baseService;
            _logger = logger;
        }

        // GET: TenantController
        public ActionResult Index()
        {
            var list = _baseService.GetByFilter(FilterAsync);

            return View(list);
        }

        [HttpPost("/DeviceAuth/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete()
        {
            try
            {
                var list = _baseService.GetByFilter(FilterAsync);
                foreach (var item in list)
                {
                    await _baseService.RemoveAsync(item.Id!);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete expired device authorization records");
                return RedirectToAction(nameof(Index));
            }
        }

        private bool FilterAsync(DeviceAuthModel model)
        {
            if (IsApproved(model)) return false;

            return IsExpired(model);
        }

        private static bool IsExpired(DeviceAuthModel model)
        {
            if (model?.RegisteredDatetime == null || model?.ExpiresIn == null) return true;

            var expirationDatetime = model.RegisteredDatetime.Value.AddSeconds(model.ExpiresIn.Value);
            return expirationDatetime < DateTime.UtcNow;
        }

        private static bool IsApproved(DeviceAuthModel model)
        {
            return model?.ApprovedDatetime != null
                && model.ApprovedDatetime > DateTime.UnixEpoch
                && model.ApprovedDatetime > model.RegisteredDatetime;

        }
    }
}
