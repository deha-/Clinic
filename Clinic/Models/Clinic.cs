using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<Appointment> Appointments { get; set; }

        public Clinic() { }

        public Clinic(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}