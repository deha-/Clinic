using Clinic.Areas.Authentication.Models;
using Clinic.DAL;
using Clinic.Enums;
using Clinic.Models;
using Clinic.ModelViews;
using Clinic.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic.Controllers
{
    public class PatientController : Controller
    {
        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;

            if (claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Role && p.Value == Clinic.Enums.RolesEnum.Patient.ToString()).Any())
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult GetPatients()
        {
            List<PatientView> result = PatientRepository.GetPatients();

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public JsonResult RemovePatient(Models.Patient patient)
        {
            PatientRepository.RemovePatient(patient.UserId);

            JsonResult result = new JsonResult();
            result.Data = patient;

            return result;
        }

        public JsonResult ActivateUser(Guid userId)
        {
            PatientRepository.ApproveUser(userId);

            JsonResult result = new JsonResult();
            result.Data = userId;

            return result;
        }
	}
}