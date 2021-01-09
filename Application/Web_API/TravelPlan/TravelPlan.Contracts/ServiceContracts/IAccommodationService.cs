using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IAccommodationService
    {
        Task<AccommodationDTO> CreateAccommodation(AccommodationCreateDTO newAccommodation);
        void DeleteAccommodation(int accommodationId);
        Task<AccommodationDTO> EditAccommodationInfo(AccommodationEditDTO accommodationInfo);
    }
}
