using Clinic.Areas.Authentication.Controllers;
using Clinic.Enums;
using Clinic.Helpers;
using Clinic.Models;
using Clinic.ModelViews;
using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic.Controllers
{
    public class ScheduleController : Controller
    {
        public ActionResult GetScheduleByDoctor()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
            Guid userId = Guid.Parse(claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            List<ScheduleView> schedules = ScheduleRepository.GetScheduleByDoctor(userId);

            ActionResult data = Json(new
            {
                data = schedules.ToArray(),
                itemsCount = schedules.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public ActionResult GetDayScheduleByDoctor(int clinicId, int day)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
            Guid userId = Guid.Parse(claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value);

            DayOfWeek dayOfTheWeek = (DayOfWeek)day;

            List<Schedule> schedules = ScheduleRepository.GetDayScheduleByDoctor(userId, clinicId, dayOfTheWeek);

            ActionResult data = Json(new
            {
                data = schedules.ToArray(),
                itemsCount = schedules.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public ActionResult GetDays()
        {
            Dictionary<int, string> days = new Dictionary<int, string>();
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
                days.Add((int)day, day.ToString());

            ActionResult data = Json(new
            {
                data = days.ToArray(),
                itemsCount = days.Count
            },
                JsonRequestBehavior.AllowGet);

            return data;
        }

        public JsonResult AddSchedule(int clinicId, int day, TimeSpan from, TimeSpan to)
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
            Guid userId = Guid.Parse(claims.Where(p => p.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault().Value);


            Schedule schedule = new Schedule(clinicId, (DayOfWeek)day, from, to);
            int status = ScheduleRepository.AddSchedule(schedule, userId);

            JsonResult result = new JsonResult();
                result.Data = ScheduleHelper.GetAddUpdateScheduleDescription(status);

            return result;
        }

        public JsonResult UpdateSchedule(Schedule schedule)
        {
            ScheduleRepository.UpdateSchedule(schedule, User.Identity.Name);

            JsonResult result = new JsonResult();
            result.Data = schedule;

            return result;
        }

        public JsonResult RemoveSchedule(int scheduleId)
        {
            int status = ScheduleRepository.RemoveSchedule(scheduleId);

            JsonResult result = new JsonResult();
            result.Data = ScheduleHelper.GetRemoveScheduleDescription(status);

            return result;
        }
	}
}