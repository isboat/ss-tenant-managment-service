using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Tenancy.Management.Models.Signalr;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Services;

namespace Tenancy.Management.Web.Controllers
{
    public class TvAppsController : Controller
    {
        private readonly ISignalrService _signalrService;
        private readonly ITenantModelService<SignalrConnectionModel> _signalrModelService;

        public TvAppsController(
            ISignalrService signalrService, 
            ITenantModelService<SignalrConnectionModel> signalrModelService)
        {
            _signalrService = signalrService;
            _signalrModelService = signalrModelService;
        }

        [HttpGet("{tenantId}/apps/running")]
        public async Task<IActionResult> Index(string tenantId)
        {            
            var model = await _signalrModelService.GetAllAsync(tenantId) ?? new List<SignalrConnectionModel>();
            return View(model);
        }

        [HttpPost("{tenantId}/apps/{id}/operator-info")]
        public async Task<IActionResult> OperatorInfo(string tenantId, string id, [FromQuery]string message)
        {
            if(string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", nameof(TenantsController));
            }
            else
            {
                if(string.IsNullOrEmpty(message))
                {
                    return BadRequest();
                }

                await SendMessage(tenantId, id, SignalRMessageType.OperatorInfo, message);
                return Ok();
            }
        }

        [HttpPost("{tenantId}/apps/{id}/terminate")]
        public async Task<IActionResult> Terminate(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", nameof(TenantsController));
            }
            else
            {
                await SendMessage(tenantId, id, SignalRMessageType.AppTerminate);
                return Ok();
            }
        }

        [HttpPost("{tenantId}/apps/{id}/force-app-upgrade")]
        public async Task<IActionResult> ForceAppUpgrade(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", nameof(TenantsController));
            }
            else
            {
                await SendMessage(tenantId, id, SignalRMessageType.AppUpgradeForce);
                return Ok();
            }
        }

        [HttpPost("{tenantId}/apps/{id}/restart-app")]
        public async Task<IActionResult> RestartApp(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return RedirectToAction("Index", nameof(TenantsController));
            }
            else
            {
                await SendMessage(tenantId, id, SignalRMessageType.AppRestart);
                return Ok();
            }
        }

        private async Task SendMessage(string tenantId, string id, string messageType, string? messageInfo = null)
        {
            var builder = Builders<SignalrConnectionModel>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(tenantId)) filter = builder.And(builder.Eq(r => r.TenantId, tenantId));
            if (!string.IsNullOrEmpty(id)) filter = builder.And(builder.Eq(r => r.Id, id));

            var filterResponse = await _signalrModelService.GetByFilterAsync(tenantId, filter);
            if (filterResponse != null && filterResponse.Any())
            {
                var model = filterResponse.FirstOrDefault();
                await _signalrService.SendMessage(new ChangeMessage
                {
                    TenantId = tenantId,
                    DeviceId = model?.DeviceId,
                    MessageType = messageType,
                    MessageData = messageInfo
                });
            }
        }
    }
}
