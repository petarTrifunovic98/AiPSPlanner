using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class WalkDecorator : Walk
    {
        public new Walk Decorator { get; set; }
        public int Lvl1DependId { get; set; }

        public override int GetLvl1DependId()
        {
            return this.Lvl1DependId;
        }
    }
}
