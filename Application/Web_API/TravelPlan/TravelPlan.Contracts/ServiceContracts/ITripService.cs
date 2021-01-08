using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface ITripService
    {
        Task<TripDTO> CreateTrip(int userId, TripCreateDTO newTrip);
        Task<TripDTO> EditTripInfo(TripEditDTO tripInfo);
        Task<bool> RemoveUserFromTrip(int tripId, int userId);
        Task<TripDTO> AddMemberToTrip(int tripId, int memberId, bool IsTeam);
    }
}
