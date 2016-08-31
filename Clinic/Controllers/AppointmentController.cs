using Clinic.Areas.Authentication.Controllers;
using Clinic.Helpers;
using Clinic.Models;
using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web.Security;

namespace Clinic.Controllers
{
    public class AppointmentController : Controller
    {
        public ActionResult GetAppointments()
        {
            AppointmentRepository.CleanAppointmentsList();

            List<Appointment> result = AppointmentRepository.GetAppointments();

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public ActionResult GetAppointmentsByDoctor()
        {
            List<Appointment> result = AppointmentRepository.GetAppointmentsByDoctor(new Guid(User.Identity.GetUserId()));

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public ActionResult GetAppointmentsByPatient()
        {
            AppointmentRepository.CleanAppointmentsList();

            List<Appointment> result = AppointmentRepository.GetAppointmentsByPatient(new Guid(User.Identity.GetUserId()));

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public JsonResult AddAppointment(int ClinicId, Guid DoctorId, string Date, string Time)
        {
            DateTime dt = new DateTime(Int32.Parse(Date.Split('-')[2]), Int32.Parse(Date.Split('-')[1]), Int32.Parse(Date.Split('-')[0]), 
                Int32.Parse(Time.Split(':')[0]), Int32.Parse(Time.Split(':')[1]), 0);
            Appointment appointment = new Appointment(ClinicId, DoctorId, dt);

            int status = AppointmentRepository.AddAppointment(appointment, User.Identity.Name);

            JsonResult result = new JsonResult();
            result.Data = AppointmentHelper.GetAddAppointmentDescription(status);

            return result;
        }

        public JsonResult ConfirmAppointment(int appointmentId)
        {
            AppointmentRepository.ConfirmAppointment(appointmentId);

            JsonResult result = new JsonResult();

            return result;
        }

        [HttpPost]
        public ActionResult GetAvailableAppointmentsByDate(int day, int month, int year)
        {
            AppointmentRepository.CleanAppointmentsList();

            DateTime dt = new DateTime(year, month, day);
            List<Appointment>  result = AppointmentRepository.GetAppointments(dt);

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        [HttpPost]
        public ActionResult GetFirstAppointmentByClinic(int clinicId)
        {
            AppointmentRepository.CleanAppointmentsList();

            List<Appointment> result = new List<Appointment>();
            Appointment appointment = AppointmentRepository.GetFirstAppointmentByClinic(clinicId);
            if (appointment != null)
                result.Add(appointment);

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        [HttpPost]
        public ActionResult GetFirstAppointmentByDoctor(Guid doctorId)
        {
            AppointmentRepository.CleanAppointmentsList();

            List<Appointment> result = new List<Appointment>();
            Appointment appointment = AppointmentRepository.GetFirstAppointmentByDoctor(doctorId);
            if (appointment != null)
                result.Add(appointment);

            ActionResult data = Json(new
            {
                data = result.ToArray(),
                itemsCount = result.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public JsonResult RemoveAppointment(Appointment appointment)
        {
            int status = AppointmentRepository.RemoveAppointment(appointment.ClinicId, appointment.DoctorId, appointment.Date);

            JsonResult result = new JsonResult();

            if (status == 0)
                result.Data = Resources.AppointmentMassages.AppointmentDeleted;
            else
                result.Data = Resources.AppointmentMassages.AppointmentDeletingError;

            return result;
        }
	}
}