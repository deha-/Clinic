using Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.ModelViews
{
    public class ScheduleView
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public List<Schedule> schedules { get; set; }

        public ScheduleView()
        {
            this.schedules = new List<Schedule>();
        }
    }
}