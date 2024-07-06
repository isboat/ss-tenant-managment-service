using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Models;

namespace Tenancy.Management.Web.Controllers
{
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<ActionResult> Index()
        {
            var list = await _registrationService.GetTenantsAsync();

            return View(list);
        }

        public async Task<ActionResult> Details(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(Index));
            }

            var model = await _registrationService.GetTenantAsync(id);
            if(model == null) return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(Index));
            }

            var model = await _registrationService.GetTenantAsync(id);
            if (model != null)
            {
                await _registrationService.RemoveAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
