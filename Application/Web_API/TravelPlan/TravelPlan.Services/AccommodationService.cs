using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Services
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccommodationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccommodationDTO> CreateAccommodation(AccommodationCreateDTO newAccommodation)
        {
            using(_unitOfWork)
            {
                Accommodation accommodation = _mapper.Map<AccommodationCreateDTO, Accommodation>(newAccommodation);
                Location location = await _unitOfWork.LocationRepository.FindByID(newAccommodation.LocationId);

                if (!DateManagerService.checkDates(location.From, location.To, accommodation.From, accommodation.To))
                    return null;

                accommodation.Location = location;

                if (location.Accommodations == null)
                    location.Accommodations = new List<Accommodation>();
                location.Accommodations.Add(accommodation);

                await _unitOfWork.AccommodationRepository.Create(accommodation);
                _unitOfWork.Save();

                return _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
            }
        }

        public void DeleteAccommodation(int accommodationId)
        {
            using (_unitOfWork)
            {
                Accommodation stubAccommodation = new Accommodation { AccommodationId = accommodationId };
                _unitOfWork.AccommodationRepository.Delete(stubAccommodation);
                _unitOfWork.Save();
            }
        }

        public async Task<AccommodationDTO> EditAccommodationInfo(AccommodationEditDTO accommodationInfo)
        {
            using (_unitOfWork)
            {
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.FindByID(accommodationInfo.AccommodationId);
                Location location = await _unitOfWork.LocationRepository.FindByID(accommodation.LocationId);

                accommodation.Type = accommodationInfo.Type;
                accommodation.Name = accommodationInfo.Name;
                accommodation.Description = accommodationInfo.Description;
                accommodation.From = accommodationInfo.From;
                accommodation.To = accommodationInfo.To;
                accommodation.Address = accommodationInfo.Address;

                if (!DateManagerService.checkDates(location.From, location.To, accommodation.From, accommodation.To))
                    return null;

                _unitOfWork.Save();

                return _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
            }
        }
    }
}
