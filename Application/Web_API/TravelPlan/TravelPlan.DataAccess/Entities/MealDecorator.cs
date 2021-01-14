using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class MealDecorator : Meal
    {
        public new Meal Decorator { get; set; }
        public int Lvl2DependId { get; set; }

        public override int GetLvl2DependId()
        {
            return this.Lvl2DependId;
        }
    }
}
