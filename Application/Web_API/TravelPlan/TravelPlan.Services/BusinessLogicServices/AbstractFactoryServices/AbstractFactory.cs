using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Services.BusinessLogicServices.AbstractFactoryServices
{
    public abstract class AbstractFactory
    {
        public abstract AddOn CreateAddOn();
        public abstract TripType CreateTripType();
        protected void AddVotable(AddOn addOn)
        {
            addOn.Votable = new Votable();
        }
    }
}
