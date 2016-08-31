using Clinic.DAL;
using Clinic.Models;
using Clinic.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Repositories
{
    public class ScheduleRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static List<Schedule> GetScheduleByDoctor(string UserName)
        {
            List<Schedule> schedules = db.Schedules.Join(db.Users, p => p.DoctorId, u => u.UserId, (p, u) => new { Schedule = p, User = u }).Where(e => e.User.Login == UserName).Select(e => e.Schedule).ToList();

            return schedules;
        }

        public static List<ScheduleView> GetScheduleByDoctor(Guid doctorId)
        {
            List<ScheduleView> schedules = new List<ScheduleView>();

            List<ScheduleView> clinics = db.DoctorsClinics.Join(db.Clinics, dc => dc.ClinicId, c => c.Id, (dc, c) => new { Clinic = c, DoctorClinic = dc }).Where(p => p.DoctorClinic.DoctorId == doctorId).Select(p => new ScheduleView
            {
                ClinicId = p.Clinic.Id,
                ClinicName = p.Clinic.Name
            }).ToList();

            foreach (ScheduleView clinic in clinics)
            {
                clinic.schedules.AddRange(db.Schedules.Where(p => p.DoctorId == doctorId && p.ClinicId == clinic.ClinicId).OrderBy(p => p.Day).ThenBy(p => p.From).ToList());
                schedules.Add(clinic);
            }

            //List<ScheduleView> schedules = db.Schedules.Join(db.Users, p => p.DoctorId, u => u.UserId, (p, u) => new { Schedule = p, User = u }).Where(e => e.User.Login == UserName).Select(e => e.Schedule).ToList();

            return schedules;
        }

        public static List<Schedule> GetDayScheduleByDoctor(Guid doctorId, int clinicId, DayOfWeek day)
        {
            List<Schedule> schedules = db.Schedules.Where(p => p.DoctorId == doctorId && p.ClinicId == clinicId && p.Day == day).ToList();

            return schedules;
        }

        public static int AddSchedule(Schedule schedule, String UserName)
        {
            int status = CanAddSchedule(schedule, UserName);
            if (status == 0)
            {
                schedule.DoctorId = UserRepository.GetUserId(UserName);
                db.Schedules.Add(schedule);
                db.SaveChanges();
            }

            return status;
        }

        public static int AddSchedule(Schedule schedule, Guid doctorId)
        {
            int status = CanAddSchedule(schedule, doctorId);
            if (status == 0)
            {
                schedule.DoctorId = doctorId;
                db.Schedules.Add(schedule);
                db.SaveChanges();
            }

            return status;
        }

        public static int UpdateSchedule(Schedule schedule, string UserName)
        {
            int status = CanAddSchedule(schedule, UserName);
            if (status == 0)
            {
                if (schedule.Id == 0)
                    db.Schedules.Add(schedule);
                else
                {
                    Schedule oldSchedule = db.Schedules.Find(schedule.Id);

                    if (oldSchedule != null)
                        db.Entry(oldSchedule).CurrentValues.SetValues(schedule);
                }

                db.SaveChanges();
            }

            return status;
        }

        public static int RemoveSchedule(int scheduleId)
        {
            Schedule schedule = db.Schedules.Find(scheduleId);

            if (schedule != null)
            {
                if (CanRemoveSchedule(schedule))
                {
                    db.Schedules.Remove(schedule);
                    db.SaveChanges();

                    return 0;
                }
                else
                    return -1;
            }

            return -2;
        }

        private static bool CanRemoveSchedule(Schedule schedule)
        {
            List<Appointment> appointments = db.Appointments.ToList();

            foreach (Appointment appointment in appointments)
            {
                //appointment in the future && the same doctor
                if (appointment.Date > DateTime.Now && appointment.DoctorId == schedule.DoctorId 
                    && appointment.Date.TimeOfDay >= schedule.From && appointment.Date.TimeOfDay <= schedule.To)
                    return false;
            }

            return true;
        }

        private static int CanAddSchedule(Schedule schedule, string UserName)
        {
            if (schedule.From >= schedule.To)
                return -1;

            List<Schedule> schedules = db.Schedules.Join(db.Users, p => p.DoctorId, u => u.UserId, (p, u) => new { Schedule = p, User = u }).Where(e => e.User.Login == UserName).Select(e => e.Schedule).ToList();
            if (schedules != null)
                if (schedules.Where(p => p.Day == schedule.Day).Where(p => schedule.From >= p.From && schedule.From <= p.To || schedule.To >= p.From && schedule.To <= p.To).Any())
                    return -2;

            return 0;
        }

        private static int CanAddSchedule(Schedule schedule, Guid doctorId)
        {
            if (schedule.From >= schedule.To)
                return -1;

            List<Schedule> schedules = db.Schedules.Where(p => p.DoctorId == doctorId).ToList();
            if (schedules != null)
                if (schedules.Where(p => p.Day == schedule.Day).Where(p => schedule.From >= p.From && schedule.From <= p.To || schedule.To >= p.From && schedule.To <= p.To).Any())
                    return -2;

            return 0;
        }
    }
}