using Clinic.Helpers;
using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Clinic.Controllers
{
    public class ClinicController : Controller
    {
        public ActionResult GetClinics()
        {
            List<Models.Clinic> result = ClinicRepository.GetClinics();

            ActionResult data = Json(new
            {                
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public JsonResult AddClinic(Models.Clinic clinic)
        {
            int status = ClinicRepository.AddClinic(clinic);

            JsonResult result = new JsonResult();
            result.Data = ClinicHelper.GetAddUpdateClinicDescription(status);

            return result;
        }

        public JsonResult UpdateClinic(Models.Clinic clinic)
        {
            int status = ClinicRepository.UpdateClinic(clinic);

            JsonResult result = new JsonResult();
            result.Data = ClinicHelper.GetAddUpdateClinicDescription(status);

            return result;
        }

        public JsonResult RemoveClinic(Models.Clinic clinic)
        {
            int status = ClinicRepository.RemoveClinic(clinic.Id);

            JsonResult result = new JsonResult();
            result.Data = ClinicHelper.GetAddUpdateClinicDescription(status); ;

            return result;
        }
	}
}