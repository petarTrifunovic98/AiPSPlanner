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
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LocationDTO> CreateLocation(LocationCreateDTO newLocation)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(newLocation.From, newLocation.To))
                    return null;

                Location location = _mapper.Map<LocationCreateDTO, Location>(newLocation);
                Trip trip = await _unitOfWork.TripRepository.FindByID(location.TripId);

                if (!DateManagerService.checkParentChildDates(trip.From, trip.To, location.From, location.To))
                    return null;

                location.Trip = trip;

                if (trip.Locations == null)
                    trip.Locations = new List<Location>();
                trip.Locations.Add(location);

                Votable votable = new Votable();
                location.Votable = votable;

                await _unitOfWork.VotableRepository.Create(votable);
                await _unitOfWork.LocationRepository.Create(location);
                _unitOfWork.Save();

                return _mapper.Map<Location, LocationDTO>(location);
            }
        }

        public async Task<bool> DeleteLocation(int locationId)
        {
            using (_unitOfWork)
            {
                Location location = await _unitOfWork.LocationRepository.GetLocationWithAccommodations(locationId);            
                foreach(Accommodation accommodation in location.Accommodations)
                {
                    _unitOfWork.VotableRepository.Delete(accommodation.VotableId);
                    _unitOfWork.AccommodationRepository.Delete(accommodation.AccommodationId);
                }
                _unitOfWork.VotableRepository.Delete(location.VotableId);
                _unitOfWork.LocationRepository.Delete(locationId);
                _unitOfWork.Save();
                return true;
            }
        }

        public async Task<LocationDTO> EditLocationInfo(LocationEditDTO locationInfo)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(locationInfo.From, locationInfo.To))
                    return null;

                Location location = await _unitOfWork.LocationRepository.FindByID(locationInfo.LocationId);
                Trip trip = await _unitOfWork.TripRepository.FindByID(location.TripId);

                location.Name = locationInfo.Name;
                location.Description = locationInfo.Description;
                location.From = locationInfo.From;
                location.To = locationInfo.To;

                if (!DateManagerService.checkParentChildDates(trip.From, trip.To, location.From, location.To))
                    return null;

                _unitOfWork.Save();

                return _mapper.Map<Location, LocationDTO>(location);
            }
        }

        public async Task<LocationDTO> GetSpecificLocation(int locationId)
        {
            using (_unitOfWork)
            {
                Location location = await _unitOfWork.LocationRepository.GetLocationWithVotable(locationId);
                return _mapper.Map<Location, LocationDTO>(location);
            }
        }

        public async Task<List<LocationDTO>> GetLocationsForTrip(int tripId)
        {
            using (_unitOfWork)
            {
                Trip trip = await _unitOfWork.TripRepository.GetTripWithILocations(tripId);
                return trip.Locations.Select(l => _mapper.Map<Location, LocationDTO>(l)).ToList();
            }
        }
    }
}
