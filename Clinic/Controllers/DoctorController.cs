using Clinic.Models;
using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic.Controllers
{
    public class DoctorController : Controller
    {
        public ActionResult Index()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
                        
            if(claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.Role && p.Value == Clinic.Enums.RolesEnum.Doctor.ToString()).Any())
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult GetDoctors()
        {
            List<Doctor> result = DoctorRepository.GetDoctors();

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public ActionResult GetDoctorsClinics()
        {
            List<DoctorClinic> result = DoctorRepository.GetDoctorsClinics();

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public ActionResult GetDoctorClinics()
        {
            List<Models.Clinic> clinics = DoctorRepository.GetDoctorClinics(User.Identity.Name);

            ActionResult data = Json(new
            {
                data = clinics.ToArray(),
                itemsCount = clinics.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public JsonResult AddDoctor(Doctor doctor)
        {
            DoctorRepository.AddDoctor(doctor);

            JsonResult result = new JsonResult();
            result.Data = doctor;

            return result;

        }

        public JsonResult RemoveDoctor(Doctor doctor)
        {
            DoctorRepository.RemoveDoctor(doctor.UserId);

            JsonResult result = new JsonResult();
            result.Data = doctor;

            return result;
        }

        public JsonResult AddDoctorClinic(int ClinicId, Guid DoctorId)
        {
            int status = DoctorRepository.AddDoctorToClinic(DoctorId, ClinicId);

            JsonResult result = new JsonResult();
            if (status == 0)
                result.Data = Resources.DoctorMessages.DoctorOK;
            else
                result.Data = Resources.DoctorMessages.DoctorAlreadyInClinic;

            return result;
        }

        public JsonResult RemoveDoctorClinic(int ClinicId, Guid DoctorId)
        {
            DoctorRepository.RemoveDoctorFromClinic(DoctorId, ClinicId);

            JsonResult result = new JsonResult();
            result.Data = "Ok";

            return result;
        }
	}
}