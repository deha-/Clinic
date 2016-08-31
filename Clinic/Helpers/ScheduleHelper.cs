using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Helpers
{
    public class ScheduleHelper
    {
        public static string GetAddUpdateScheduleDescription(int status) 
        {
            switch (status)
            {
                case -1:
                    return Resources.ScheduleMessages.ScheduleWrongTime;
                case -2:
                    return Resources.ScheduleMessages.ScheduleConflict;
                default:
                    return Resources.ScheduleMessages.ScheduleOK;
            }
        }

        public static string GetRemoveScheduleDescription(int status) 
        {
            switch (status)
            {
                case -1:
                    return Resources.ScheduleMessages.CanNotRemoveSchedule;
                default:
                    return Resources.ScheduleMessages.ScheduleOK;
            }
        }
    }
}