using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        //Add property for each repository interface. If a certain entity (e.g. "SomeEntity") needs only basic data access methods 
        //(which are already provided by the IRepositoryBase), create a property of type IRepositoryBase<SomeEntity>.
        //For each of these properties, expose only the "get" method.
        IUserRepository UserRepository { get; }

        //----------
        ITeamRepository TeamRepository2 { get; }
        //----------
        IRepositoryBase<Team> TeamRepository { get; }
        bool Save();
    }
}
