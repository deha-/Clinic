using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Helpers
{
    public class ClinicHelper
    {
        public static string GetAddUpdateClinicDescription(int status) 
        {
            switch (status)
            {
                case -1:
                    return Resources.ClinicMessages.ClinicExists;
                case -2:
                    return Resources.ClinicMessages.ClinicWithDoctors;
                default:
                    return Resources.ClinicMessages.ClinicOK;
            }
        }
    }
}