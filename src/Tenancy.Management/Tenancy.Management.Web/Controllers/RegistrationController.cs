using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenancy.Management.Services.Interfaces;
using Tenancy.Management.Web.Models;

namespace Tenancy.Management.Web.Controllers
{
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

        // GET: RegisterationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RegisterationController/Delete/5
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
