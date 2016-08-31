using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Helpers
{
    public class DoctorHelper
    {
        public static string GetAddDoctorDescription(int status)
        {
            switch (status)
            {
                case -1:
                    return Resources.DoctorMessages.WrongPassword;
                case -2:
                    return Resources.DoctorMessages.WrongPWZ;
                case -3:
                    return Resources.DoctorMessages.LoginInUse;
                default:
                    return Resources.DoctorMessages.DoctorOK;
            }
        }
    }
}