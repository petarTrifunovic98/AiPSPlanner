using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface ILocationService
    {
        Task<LocationDTO> CreateLocation(LocationCreateDTO newLocation);
        void DeleteLocation(int locationId);
        Task<LocationDTO> EditLocationInfo(LocationEditDTO locationInfo);
    }
}
