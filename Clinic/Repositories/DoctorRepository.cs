using Clinic.Areas.Authentication.Models;
using Clinic.DAL;
using Clinic.Enums;
using Clinic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Clinic.Repositories
{
    public class DoctorRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = db.Doctors.ToList();

            return doctors;
        }

        public static List<DoctorClinic> GetDoctorsClinics()
        {
            List<DoctorClinic> doctors = db.DoctorsClinics.ToList();

            return doctors;
        }

        public static Doctor GetDoctorById(Guid DoctorId)
        {
            Doctor doctor = db.Doctors.Find(DoctorId);

            return doctor;
        }

        public static Guid AddDoctor(Doctor doctor, string login, string password)
        {
            Guid userId = UserRepository.AddUser(login, password);

            doctor.UserId = userId;
            db.Doctors.Add(doctor);
            db.SaveChanges();

            Guid roleId = db.Roles.Where(p => p.RoleName == RolesEnum.Doctor.ToString()).FirstOrDefault().RoleId;

            UserRole ur = new UserRole(userId, roleId);
            db.UserRoles.Add(ur);
            db.SaveChanges();

            return userId;
        }

        public static Guid AddDoctor(Doctor doctor)
        {
            return AddDoctor(doctor, doctor.Login, doctor.Password);
        }

        public static int AddDoctorToClinic(Guid userId, int clinicId)
        {
            if (db.DoctorsClinics.Where(p => p.DoctorId == userId && p.ClinicId == clinicId).Any())
                return -1;
            else
            {
                DoctorClinic dc = new DoctorClinic(userId, clinicId);
                db.DoctorsClinics.Add(dc);
                UserRepository.ActivateUser(userId);
                db.SaveChanges();

                return 0;
            }
        }

        public static void RemoveDoctor(Guid userId)
        {
            Doctor doctor = db.Doctors.Find(userId);
            if (doctor != null)
            {
                db.Doctors.Remove(doctor);
                db.SaveChanges();
            }

            User user = db.Users.Find(userId);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        /*public static List<UserInfo> GetDoctors()
        {
            List<UserInfo> doctors = db.UserInfo.Where(p => p.Roles.Contains(db.Roles.Where(r => r.RoleName == Roles.Doctor.ToString()).FirstOrDefault())).ToList();

            return doctors;
        }*/

        public static List<Models.Clinic> GetDoctorClinics(string UserName)
        {
            Guid userId = UserRepository.GetUserId(UserName);
            List<Models.Clinic> clinics = db.Clinics.Join(db.DoctorsClinics, cli => cli.Id, doc => doc.ClinicId, (cli, doc) => new { Clinic = cli, Doctor = doc }).Where(p => p.Doctor.DoctorId == userId).Select(p => p.Clinic).ToList();

            return clinics;
        }

        public static void RemoveDoctorFromClinic(Guid doctorId, int clinicId)
        {
            DoctorClinic dc = db.DoctorsClinics.Where(p => p.DoctorId == doctorId && p.ClinicId == clinicId).First();
            db.DoctorsClinics.Remove(dc);
            db.SaveChanges();
        }
    }
}