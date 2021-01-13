using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public abstract class CruiseDecorator : Cruise
    {
        public new Cruise Decorator { get; set; }
    }
}
