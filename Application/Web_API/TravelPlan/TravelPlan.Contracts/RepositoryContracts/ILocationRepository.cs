using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface ILocationRepository : IRepositoryBase<Location>
    {
        Task<Location> GetLocationWithVotable(int LocationId);
        Task<Location> GetLocationWithAccommodations(int LocationId);
    }
}
