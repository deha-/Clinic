using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
                        
            if(claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Role && p.Value == Clinic.Enums.RolesEnum.Admin.ToString()).Any())
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
	}
}