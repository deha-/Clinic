using Clinic.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public Guid DoctorId { get; set; }
        public int ClinicId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public Schedule() { }

        public Schedule(int ClinicId, DayOfWeek Day, TimeSpan From, TimeSpan To)
        {
            this.ClinicId = ClinicId;
            this.Day = Day;
            this.From = From;
            this.To = To;
        }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }

        [NotMapped]
        public string FromStr 
        { 
            get 
            {
                return String.Format(String.Format("{0}:{1}", this.From.Hours, String.Format("{0:00}", this.From.Minutes))); 
            } 
        }

        [NotMapped]
        public string ToStr
        {
            get
            {
                return String.Format(String.Format("{0}:{1}", this.To.Hours, String.Format("{0:00}", this.To.Minutes))); 
            }
        }

        [NotMapped]
        public string DayName
        {
            get
            {
                return this.Day.ToString();
            }
        }

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
    }
}