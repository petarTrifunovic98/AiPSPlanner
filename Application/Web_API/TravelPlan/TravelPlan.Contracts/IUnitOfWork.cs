using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ITeamRepository TeamRepository { get; }
        ITripRepository TripRepository { get; }

        bool Save();
    }
}
