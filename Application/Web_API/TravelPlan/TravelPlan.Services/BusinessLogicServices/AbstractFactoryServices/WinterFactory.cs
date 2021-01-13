using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Services.BusinessLogicServices.AbstractFactoryServices
{
    public class WinterFactory : AbstractFactory
    {
        public override AddOn CreateAddOn()
        {
            AddOn addOn = new WinterAddOn();
            AddVotable(addOn);
            return addOn;
        }

        public override TripType CreateTripType()
        {
            return new WinterType();
        }
    }
}
