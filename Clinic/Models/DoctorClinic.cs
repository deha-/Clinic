using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class DoctorClinic
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public Guid DoctorId { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required]
        public int ClinicId { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

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

        public DoctorClinic() { }

        public DoctorClinic(Guid DoctorId, int ClinicId) 
        {
            this.DoctorId = DoctorId;
            this.ClinicId = ClinicId;
        }

        //[ForeignKey("DoctorId")]
        //public virtual UserInfo Doctor { get; set; }

        //[ForeignKey("ClinicId")]
        //public virtual Clinic Clinic { get; set; }
    }
}