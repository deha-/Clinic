using Clinic.Areas.Authentication.Models;
using Clinic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Clinic.DAL
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext() : base("ClinicContext") { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Models.Clinic> Clinics { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorClinic> DoctorsClinics { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new ClinicInitializer());

            base.Configuration.LazyLoadingEnabled = false;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>().Property(p => p.Date).HasColumnType("datetime2");
            modelBuilder.Entity<Appointment>().Property(p => p.AddedDate).HasColumnType("datetime2");
            modelBuilder.Entity<User>().Property(p => p.CreateDate).HasColumnType("datetime2");
            //modelBuilder.Entity<User>().Property(p => p.).HasColumnType("datetime2");

            //modelBuilder.Entity<Appointment>().HasRequired<Doctor>(p => p.Doctor).WithMany(p => p.Appointments).HasForeignKey(p => p.DoctorId);
            //modelBuilder.Entity<Doctor>().HasMany<Appointment>(p => p.Appointments).WithRequired(p => p.Doctor).HasForeignKey(p => p.DoctorId);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}