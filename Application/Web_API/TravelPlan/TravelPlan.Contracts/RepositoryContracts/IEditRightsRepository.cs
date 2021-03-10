using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IEditRightsRepository
    {
        Task SetEditRightHolder(int tripId, int userId);
        Task<string> GetNextRightHolder(int tripId);
        Task<string> GetCurrentRightHolder(int tripId);
        Task RemoveUserFromRequestQueue(int tripId, int userId);
        Task<long> RequestTripEdit(int tripId, int userId);
        Task SetEditRightsExpiration(int tripId, int newMinutes);
    }
}
