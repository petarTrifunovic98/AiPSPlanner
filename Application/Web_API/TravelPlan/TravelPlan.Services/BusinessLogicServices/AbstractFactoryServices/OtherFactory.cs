using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Services.BusinessLogicServices.AbstractFactoryServices
{
    class OtherFactory : AbstractFactory
    {
        public override AddOn CreateAddOn()
        {
            return new OtherAddOn();
        }

        public override TripType CreateTripType()
        {
            return new OtherType();
        }
    }
}
