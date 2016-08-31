using Clinic.Areas.Authentication.Models;
using Clinic.DAL;
using Clinic.Enums;
using Clinic.Models;
using Clinic.ModelViews;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Clinic.Repositories
{
    public class PatientRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static List<PatientView> GetPatients()
        {
            List<PatientView> patients = db.Patients.Join(db.Users, u => u.UserId, p => p.UserId, (u, p) => new { Pat = p, Usr = u }).Select(p => new PatientView
            {
                UserId = p.Usr.UserId,
                IsApproved = p.Pat.IsApproved,
                FirstName = p.Usr.FirstName,
                LastName = p.Usr.LastName,
                PESEL = p.Usr.PESEL,
                Address = p.Usr.Address
            }).ToList();

            return patients;
        }

        public static List<Patient> GetNotApprovedUsers()
        {
            List<Patient> patients = db.Patients.Join(db.Users, p => p.UserId, u => u.UserId, (p, u) => new { Patient = p, User = u }).Where(e => e.User.IsApproved == false).Select(e => e.Patient).ToList();

            return patients;
        }

        public static void ApproveUser(Guid userId)
        {
            User user = db.Users.Find(userId);
            if (user != null)
            {
                user.IsApproved = true;
                db.SaveChanges();
            }
        }

        public static Guid AddPatient(Patient patient)
        {
            Guid userId = db.Patients.Add(patient).UserId;
            db.SaveChanges();

            return userId;
        }

        public static Patient GetPatientById(Guid patientId)
        {
            Patient patient = db.Patients.Find(patientId);

            return patient;
        }

        public static Guid AddPatient(Patient patient, string login, string password)
        {
            Guid userId = UserRepository.AddUser(login, password);

            patient.UserId = userId;
            db.Patients.Add(patient);
            db.SaveChanges();

            Guid roleId = db.Roles.Where(p => p.RoleName == RolesEnum.Patient.ToString()).FirstOrDefault().RoleId;

            UserRole ur = new UserRole(userId, roleId);
            db.UserRoles.Add(ur);
            db.SaveChanges();

            return userId;
        }

        public static void RemovePatient(Guid userId)
        {
            Patient patient = db.Patients.Find(userId);
            if (patient != null)
            {
                db.Patients.Remove(patient);
                db.SaveChanges();
            }

            User user = db.Users.Find(userId);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public static void UpdatePatient(Patient patient)
        {
            Patient oldPatient = db.Patients.Find(patient.UserId);

            if (oldPatient != null)
            {
                db.Entry(oldPatient).CurrentValues.SetValues(patient);
                db.SaveChanges();
            }
        }
    }
}