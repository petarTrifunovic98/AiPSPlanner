using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public abstract class BreakfastDecorator : Breakfast
    {
        public new Breakfast Decorator { get; set; }
    }
}
