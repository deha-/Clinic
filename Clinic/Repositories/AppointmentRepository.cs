using Clinic.DAL;
using Clinic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Clinic.Repositories
{
    public static class AppointmentRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static List<Appointment> GetAppointments()
        {
            List<Appointment> appointments = db.Appointments.ToList();

            return appointments;
        }

        public static List<Appointment> GetAppointments(DateTime date)
        {
            List<Appointment> appointments = new List<Appointment>();
            DayOfWeek day = date.DayOfWeek;
            TimeSpan time = DateTime.Now.TimeOfDay;

            List<Schedule> schedules = db.Schedules.Where(p => p.Day == day).OrderBy(p => p.ClinicId).OrderBy(p => p.DoctorId).ToList();
            foreach (Schedule schedule in schedules)
            {
                TimeSpan from = schedule.From;
                TimeSpan to = schedule.To;
                TimeSpan appointmentTime = new TimeSpan(0, 30, 0);

                while (from.Add(appointmentTime) < to)
                {
                    if ((date.Date.CompareTo(DateTime.Now.Date) == 0 && from > time) || date.Date != DateTime.Today)
                    {
                        Appointment appointment = new Appointment();
                        appointment.ClinicId = schedule.ClinicId;
                        appointment.DoctorId = schedule.DoctorId;
                        appointment.Date = date;
                        appointment.Date = appointment.Date + from;
                        appointments.Add(appointment);
                    }
                    from = from.Add(appointmentTime);
                }
            }

            List<Appointment> result = RemoveArrangedAppointments(appointments);

            return result;
        }

        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;

            if (target <= start)
                target += 7;

            return from.AddDays(target - start);
        }

        private static Appointment FindClosestAppointment(List<Appointment> appointments)
        {
            List<Schedule> result = new List<Schedule>();

            DateTime now = DateTime.Now;
            Appointment closestAppointment = null;
            long min = long.MaxValue;

            foreach (Appointment appointment in appointments)
            {
                if (Math.Abs(appointment.Date.Ticks - now.Ticks) < min)
                {
                    min = appointment.Date.Ticks - now.Ticks;
                    closestAppointment = appointment;
                }

            }

            return closestAppointment;
        }

        public static Appointment GetFirstAppointmentByClinic(int clinicId)
        {
            List<Appointment> appointments = new List<Appointment>();

            List<Schedule> schedules = db.Schedules.Where(p => p.ClinicId == clinicId).OrderBy(p => p.Day).ToList();
            foreach (Schedule schedule in schedules)
            {
                TimeSpan from = schedule.From;
                TimeSpan to = schedule.To;
                TimeSpan appointmentTime = new TimeSpan(0, 30, 0);

                while (from.Add(appointmentTime) < to)
                {
                    Appointment appointment = new Appointment();
                    appointment.ClinicId = schedule.ClinicId;
                    appointment.DoctorId = schedule.DoctorId;
                    appointment.Date = Next(DateTime.Now.Date, schedule.Day) + from;
                    appointments.Add(appointment);
                    from = from.Add(appointmentTime);
                }
            }

            List<Appointment> result = RemoveArrangedAppointments(appointments);

            return FindClosestAppointment(result);
        }

        public static Appointment GetFirstAppointmentByDoctor(Guid doctorId)
        {
            List<Appointment> appointments = new List<Appointment>();

            List<Schedule> schedules = db.Schedules.Where(p => p.DoctorId == doctorId).OrderBy(p => p.Day).ToList();
            foreach (Schedule schedule in schedules)
            {
                TimeSpan from = schedule.From;
                TimeSpan to = schedule.To;
                TimeSpan appointmentTime = new TimeSpan(0, 30, 0);

                while (from.Add(appointmentTime) < to)
                {
                    Appointment appointment = new Appointment();
                    appointment.ClinicId = schedule.ClinicId;
                    appointment.DoctorId = schedule.DoctorId;
                    appointment.Date = Next(DateTime.Now.Date, schedule.Day) + from;
                    appointments.Add(appointment);
                    from = from.Add(appointmentTime);
                }
            }

            List<Appointment> result = RemoveArrangedAppointments(appointments);

            return FindClosestAppointment(result);
        }

        private static List<Appointment> RemoveArrangedAppointments(List<Appointment> appointments)
        {
            List<Appointment> result = new List<Appointment>();
            List<Appointment> arrangedAppointments = db.Appointments.Where(p => p.Date >= DateTime.Now).ToList();

            TimeSpan appointmentTime = new TimeSpan(0, 30, 0);
            foreach (Appointment appointment in appointments)
            {
                if (!arrangedAppointments.Where(p => p.DoctorId == appointment.DoctorId && p.ClinicId == appointment.ClinicId
                        && (p.Date.TimeOfDay >= appointment.Date.TimeOfDay && p.Date.TimeOfDay < appointment.Date.TimeOfDay.Add(appointmentTime) && p.Date.Date == appointment.Date.Date
                        || p.Date.TimeOfDay.Add(appointmentTime) > appointment.Date.TimeOfDay && p.Date.TimeOfDay.Add(appointmentTime) <= appointment.Date.TimeOfDay.Add(appointmentTime) && p.Date.Date == appointment.Date.Date)).Any())
                {
                    result.Add(appointment);
                }
            }

            return result;
        }

        public static List<Appointment> GetAppointmentsByClinic(int clinicId)
        {
            List<Appointment> appointments = db.Appointments.Where(a => a.ClinicId == clinicId).ToList();

            return appointments;
        }

        public static List<Appointment> GetAppointmentsByDoctor(Guid doctorId)
        {
            List<Appointment> appointments = db.Appointments.Where(p => p.DoctorId == doctorId && p.IsConfirmed == true).ToList();

            return appointments;
        }

        public static List<Appointment> GetAppointmentsByPatient(Guid patientId)
        {
            List<Appointment> appointments = db.Appointments.Where(a => a.PatientId == patientId).ToList();

            return appointments;
        }

        public static int AddAppointment(Appointment appointment, String UserName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    appointment.PatientId = UserRepository.GetUserId(UserName);
                    appointment.Doctor = db.Doctors.Find(appointment.DoctorId);
                    appointment.Patient = db.Patients.Find(appointment.PatientId);
                    appointment.Clinic = db.Clinics.Find(appointment.ClinicId);
                    db.Appointments.Add(appointment);
                    db.SaveChanges();

                    transaction.Commit();

                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return -1;
                }
            }
        }

        public static void UpdateAppointment(Appointment appointment)
        {
            Appointment oldAppointment = db.Appointments.Where(p => p.ClinicId == appointment.ClinicId &&
                p.DoctorId == appointment.DoctorId && p.Date == appointment.Date).FirstOrDefault();

            if (oldAppointment == null)
                db.Appointments.Add(appointment);
            else
                db.Entry(oldAppointment).CurrentValues.SetValues(appointment);

            db.SaveChanges();
        }

        public static int RemoveAppointment(int clinicId, Guid doctorId, DateTime date)
        {
            Appointment appointment = db.Appointments.Where(p => p.ClinicId == clinicId && p.DoctorId == doctorId && p.Date == date).FirstOrDefault();
            
            if (appointment != null)
            {
                if (appointment.Date < DateTime.Now)
                    return -1;
                else
                {
                    db.Appointments.Remove(appointment);
                    db.SaveChanges();
                }
            }

            return 0;
        }

        public static void ConfirmAppointment(int appointmentId)
        {
            Appointment appointment = db.Appointments.Find(appointmentId);

            if (appointment != null)
            {
                appointment.IsConfirmed = true;
                db.SaveChanges();
            }
        }

        public static void CleanAppointmentsList()
        {
            List<Appointment> appointments = db.Appointments.Where(p => p.IsConfirmed == false).ToList();

            foreach (Appointment appointment in appointments)
            {
                if ((DateTime.Now - appointment.AddedDate).TotalMinutes > 2)
                {
                    appointment.Clinic = db.Clinics.Find(appointment.ClinicId);
                    appointment.Doctor = db.Doctors.Find(appointment.DoctorId);
                    appointment.Patient = db.Patients.Find(appointment.PatientId);

                    db.Appointments.Remove(appointment);
                    db.SaveChanges();
                }
            }
        }
    }
}