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
            return new SeaAddOn();
        }

        public override TripType CreateTripType()
        {
            return new SeaType();
        }
    }
}
