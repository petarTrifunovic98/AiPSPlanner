using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class TrainTourDecorator : TrainTour
    {
        public new TrainTour Decorator { get; set; }
        public int Lvl1DependId { get; set; }

        public override int GetLvl1DependId()
        {
            return this.Lvl1DependId;
        }
    }
}
