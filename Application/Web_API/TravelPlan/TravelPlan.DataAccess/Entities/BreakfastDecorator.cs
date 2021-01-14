using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public abstract class BreakfastDecorator : Breakfast
    {
        public new Breakfast Decorator { get; set; }
        public int Lvl2DependId { get; set; }

        public override int GetLvl2DependId()
        {
            return this.Lvl2DependId;
        }
    }
}
