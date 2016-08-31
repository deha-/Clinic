using Clinic.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Repositories
{
    public class ClinicRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static List<Models.Clinic> GetClinics()
        {
            List<Models.Clinic> clinics = db.Clinics.ToList();

            return clinics;
        }

        public static Models.Clinic GetClinicById(int ClinicId)
        {
            Models.Clinic clinic = db.Clinics.Find(ClinicId);

            return clinic;
        }

        public static int AddClinic(Models.Clinic clinic)
        {
            if (db.Clinics.Where(p => p.Name == clinic.Name).Any())
                return -1;

            int clinicId = db.Clinics.Add(clinic).Id;
            db.SaveChanges();

            return clinicId;
        }

        public static int UpdateClinic(Models.Clinic clinic)
        {
            if (clinic.Id == 0)
                db.Clinics.Add(clinic);
            else
            {
                if (db.Clinics.Where(p => p.Name == clinic.Name).Any())
                    return -1;

                Models.Clinic oldClinic = db.Clinics.Find(clinic.Id);

                if (oldClinic != null)
                    db.Entry(oldClinic).CurrentValues.SetValues(clinic);
            }

            db.SaveChanges();

            return 0;
        }

        public static int RemoveClinic(int clinicId)
        {
            if (db.DoctorsClinics.Where(p => p.ClinicId == clinicId).Any())
                return -2;

            Models.Clinic clinic = db.Clinics.Find(clinicId);

            if (clinic != null)
            {
                db.Clinics.Remove(clinic);
                db.SaveChanges();
            }

            return 0;
        }
    }
}