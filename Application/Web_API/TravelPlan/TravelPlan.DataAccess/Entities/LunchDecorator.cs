using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public abstract class LunchDecorator : Lunch
    {
        public new Lunch Decorator { get; set; }
        public int Lvl2DependId { get; set; }
    }
}
