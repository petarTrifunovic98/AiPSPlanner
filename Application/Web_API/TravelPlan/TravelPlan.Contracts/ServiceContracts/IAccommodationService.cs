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
        Task<bool> DeleteAccommodation(int accommodationId);
        Task<AccommodationDTO> EditAccommodationInfo(AccommodationEditDTO accommodationInfo);
        IEnumerable<string> GetAccommodationTypes();
        Task<AccommodationDTO> GetSpecificAccommodation(int accommodationId);
        Task<List<AccommodationDTO>> GetAccommodationsForLocation(int locationId);
        Task<AccommodationPictureDTO> AddAccommodationPicture(AccommodationPictureCreateDTO picture);
        Task<IEnumerable<AccommodationPictureDTO>> GetAccommodationPictures(int accommodationId);
        Task DeleteAccommodationPicture(int pictureId);
    }
}
