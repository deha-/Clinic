using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Doctor
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(7, MinimumLength = 7)]
        public string PWZ { get; set; }

        //public virtual ICollection<Appointment> Appointments { get; set; }

        [NotMapped]
        public string Login { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public Doctor() { }

        public Doctor(string FirstName, string LastName, string PWZ)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PWZ = PWZ;
        }

        public Doctor(string FirstName, string LastName, string PWZ, string Login, string Password)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PWZ = PWZ;
            this.Login = Login;
            this.Password = Password;
        }
    }
}