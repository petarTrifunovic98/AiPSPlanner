using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface ITripRepository : IRepositoryBase<Trip>
    {
        Task<Trip> GetTripWithMembers(int id);
        Task<Trip> GetTripWithMembersAndLocations(int id);
        Task<Trip> GetTripWithItemsAndMembers(int id);
        Task<Trip> GetTripWithILocations(int id);
        Task<IEnumerable<Trip>> GetUserTrips(User user);
    }
}
