using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Services
{
    public class DateManagerService
    {
        public static bool checkDates(DateTime fromParent, DateTime toParent, DateTime fromChild, DateTime toChild)
        {
            return fromChild >= fromParent && toChild <= toParent;
        }
    }
}
