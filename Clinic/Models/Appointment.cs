using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Appointment
    {
        [Key]
        [Column(Order = 1)]
        public int ClinicId { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        [Key]
        [Column(Order = 3)]
        public DateTime Date { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsConfirmed { get; set; }

        public Appointment() { }

        public Appointment(int ClinicId, Guid DoctorId, DateTime Date) 
        {
            this.ClinicId = ClinicId;
            this.DoctorId = DoctorId;
            this.Date = Date;
            this.AddedDate = DateTime.Now;
            this.IsConfirmed = false;
        }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [NotMapped]
        public string ClinicName
        {
            get
            {
                if (this.Clinic == null)
                    this.Clinic = ClinicRepository.GetClinicById(this.ClinicId);

                return this.Clinic.Name;
            }
        }

        [NotMapped]
        public string DoctorName
        {
            get
            {
                if (this.Doctor == null)
                    this.Doctor = DoctorRepository.GetDoctorById(this.DoctorId);
                    
                return this.Doctor.FirstName + " " + this.Doctor.LastName;
            }
        }

        [NotMapped]
        public string PatientName
        {
            get
            {
                if (this.Patient == null)
                    this.Patient = PatientRepository.GetPatientById(this.PatientId);

                if (this.Patient != null)
                    return this.Patient.FirstName + " " + this.Patient.LastName;
                else
                    return "";
            }
        }

        [NotMapped]
        public string DateName
        {
            get
            {
                return this.Date.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [NotMapped]
        public string AddedDateName
        {
            get
            {
                return this.AddedDate.ToString("dd-MM-yyyy HH:mm:ss");
            }
        }
    }
}