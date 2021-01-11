using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Helpers;

namespace TravelPlan.Services.BusinessLogicServices
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
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(newAccommodation.From, newAccommodation.To))
                    return null;

                Accommodation accommodation = _mapper.Map<AccommodationCreateDTO, Accommodation>(newAccommodation);
                Location location = await _unitOfWork.LocationRepository.FindByID(newAccommodation.LocationId);

                if (!DateManagerService.checkParentChildDates(location.From, location.To, accommodation.From, accommodation.To))
                    return null;

                accommodation.Location = location;

                if (location.Accommodations == null)
                    location.Accommodations = new List<Accommodation>();
                location.Accommodations.Add(accommodation);

                Votable votable = new Votable();
                accommodation.Votable = votable;

                await _unitOfWork.VotableRepository.Create(votable);
                await _unitOfWork.AccommodationRepository.Create(accommodation);
                _unitOfWork.Save();

                return _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
            }
        }

        public async Task<bool> DeleteAccommodation(int accommodationId)
        {
            using (_unitOfWork)
            {
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.FindByID(accommodationId);
                _unitOfWork.VotableRepository.Delete(accommodation.VotableId);
                _unitOfWork.AccommodationRepository.Delete(accommodationId);
                _unitOfWork.Save();
                return true;
            }
        }

        public async Task<AccommodationDTO> EditAccommodationInfo(AccommodationEditDTO accommodationInfo)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(accommodationInfo.From, accommodationInfo.To))
                    return null;

                Accommodation accommodation = await _unitOfWork.AccommodationRepository.FindByID(accommodationInfo.AccommodationId);
                Location location = await _unitOfWork.LocationRepository.FindByID(accommodation.LocationId);

                accommodation.Type = accommodationInfo.Type;
                accommodation.Name = accommodationInfo.Name;
                accommodation.Description = accommodationInfo.Description;
                accommodation.From = accommodationInfo.From;
                accommodation.To = accommodationInfo.To;
                accommodation.Address = accommodationInfo.Address;

                if (!DateManagerService.checkParentChildDates(location.From, location.To, accommodation.From, accommodation.To))
                    return null;

                _unitOfWork.Save();

                return _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
            }
        }

        public async Task<AccommodationDTO> GetSpecificAccommodation(int accommodationId)
        {
            using(_unitOfWork)
            {
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.GetAccommodationWithVotable(accommodationId);
                return _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
            }
        }

        public async Task<List<AccommodationDTO>> GetAccommodationsForLocation(int locationId)
        {
            using (_unitOfWork)
            {
                Location location = await _unitOfWork.LocationRepository.GetLocationWithAccommodations(locationId);
                return location.Accommodations.Select(a => _mapper.Map<Accommodation, AccommodationDTO>(a)).ToList();
            }
        }
    }
}
