using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IEditRightsService
    {
        Task<bool> HasEditRights(int tripId);
        Task<bool> RequestTripEdit(int tripId, int userId);
        Task ReleaseTripEdit(int tripId);
        Task LeaveRequestEditQueue(int tripId, int userId);
        Task ProlongEditRights(int tripId, int minutes);
    }
}
