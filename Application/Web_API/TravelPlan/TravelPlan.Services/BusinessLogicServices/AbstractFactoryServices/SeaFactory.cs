using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Services.BusinessLogicServices.AbstractFactoryServices
{
    public class SeaFactory : AbstractFactory
    {
        public override AddOn CreateAddOn()
        {
            AddOn addOn = new SeaAddOn();
            AddVotable(addOn);
            return addOn;
        }

        public override TripType CreateTripType()
        {
            return new SeaType();
        }
    }
}
