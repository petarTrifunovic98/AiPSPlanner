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
            return new WinterAddOn();
        }

        public override TripType CreateTripType()
        {
            return new WinterType();
        }
    }
}
