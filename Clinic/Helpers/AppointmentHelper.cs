using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Helpers
{
    public class AppointmentHelper
    {
        public static string GetAddAppointmentDescription(int status) 
        {
            switch (status)
            {
                case -1:
                    return Resources.AppointmentMassages.AppointmentTaken;
                default:
                    return Resources.AppointmentMassages.AppointmentOk;
            }
        }
    }
}