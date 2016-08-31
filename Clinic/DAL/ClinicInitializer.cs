using Clinic.Areas.Authentication.Models;
using Clinic.Enums;
using Clinic.Models;
using Clinic.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Clinic.DAL
{
    public class ClinicInitializer : DropCreateDatabaseIfModelChanges<ClinicDbContext>
    {
        /*public override void InitializeDatabase(ClinicDbContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            base.InitializeDatabase(context);
        }*/

        protected override void Seed(ClinicDbContext context)
        {
            UserRoleStore us = new UserRoleStore(context);

            //roles
            foreach (var role in Enum.GetValues(typeof(RolesEnum)).Cast<RolesEnum>())
                us.CreateAsync(new Role(role.ToString()));


            //admin
            User admin = new User("admin", "admin");
            us.CreateAsync(admin);
            us.AddToRoleAsync(admin, RolesEnum.Admin.ToString());
            UserRepository.ActivateUser("admin");


            //clinics
            ClinicRepository.AddClinic(new Models.Clinic(1, "Princeton-Plainsboro Teaching Hospital"));
            ClinicRepository.AddClinic(new Models.Clinic(2, "County General Hospital in Chicago"));
            ClinicRepository.AddClinic(new Models.Clinic(3, "Seattle Grace Hospital"));


            //doctors
            Guid doctorId = DoctorRepository.AddDoctor(new Doctor("Gregory", "House", "1111111"), "house", "house");
            DoctorRepository.AddDoctorToClinic(doctorId, 1);
            doctorId = DoctorRepository.AddDoctor(new Doctor("James", "Wilson", "2222222"), "wilson", "wilson");
            DoctorRepository.AddDoctorToClinic(doctorId, 1);
            doctorId = DoctorRepository.AddDoctor(new Doctor("Mark", "Greene", "3333333"), "mark", "mark");
            DoctorRepository.AddDoctorToClinic(doctorId, 2);
            doctorId = DoctorRepository.AddDoctor(new Doctor("Susan", "Lewis", "4444444"), "susan", "susan");
            DoctorRepository.AddDoctorToClinic(doctorId, 2);
            doctorId = DoctorRepository.AddDoctor(new Doctor("Meredith", "Grey", "5555555"), "grey", "grey");
            DoctorRepository.AddDoctorToClinic(doctorId, 3);
            doctorId = DoctorRepository.AddDoctor(new Doctor("Derek", "Shepherd", "6666666"), "derek", "derek");
            DoctorRepository.AddDoctorToClinic(doctorId, 3);


            //doctor schedule
            ScheduleRepository.AddSchedule(new Schedule(1, DayOfWeek.Monday, new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0)), "house");
            ScheduleRepository.AddSchedule(new Schedule(1, DayOfWeek.Thursday, new TimeSpan(16, 0, 0), new TimeSpan(18, 30, 0)), "house");
            ScheduleRepository.AddSchedule(new Schedule(3, DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0)), "grey");
            ScheduleRepository.AddSchedule(new Schedule(3, DayOfWeek.Tuesday, new TimeSpan(8, 0, 0), new TimeSpan(11, 0, 0)), "grey");

            //patient
            PatientRepository.AddPatient(new Patient("Joanna", "Moskwa", "81051212260", "os. Osiedlowe", "Poznań", "60-000"), "joanna", "joanna");
            UserRepository.ActivateUser("joanna");


            base.Seed(context);
        }
    }
}