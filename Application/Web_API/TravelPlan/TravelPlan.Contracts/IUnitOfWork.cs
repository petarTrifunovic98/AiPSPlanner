using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        //Add property for each repository interface. If a certain entity (e.g. "SomeEntity") needs only basic data access methods 
        //(which are already provided by the IRepositoryBase), create a property of type IRepositoryBase<SomeEntity>.
        //For each of these properties, expose only the "get" method.
        void Save();
    }
}
