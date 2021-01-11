using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Helpers
{
    public class DateManagerService
    {
        public static bool checkParentChildDates(DateTime fromParent, DateTime toParent, DateTime fromChild, DateTime toChild)
        {
            return fromChild >= fromParent && toChild <= toParent;
        }

        public static bool checkFromToDates(DateTime from, DateTime to)
        {
            return from <= to;
        }
    }
}
