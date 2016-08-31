using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;

            if (claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Role && p.Value == Clinic.Enums.RolesEnum.Admin.ToString()).Any())
                return RedirectToAction("Index", "Admin");
            if (claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Role && p.Value == Clinic.Enums.RolesEnum.Doctor.ToString()).Any())
                return RedirectToAction("Index", "Doctor");
            if (claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Role && p.Value == Clinic.Enums.RolesEnum.Patient.ToString()).Any())
                return RedirectToAction("Index", "Patient");

            return View();
        }
    }
}