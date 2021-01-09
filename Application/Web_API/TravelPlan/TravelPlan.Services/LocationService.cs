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
            using(_unitOfWork)
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

                await _unitOfWork.LocationRepository.Create(location);
                _unitOfWork.Save();

                return _mapper.Map<Location, LocationDTO>(location);
            }
        }

        public void DeleteLocation(int locationId)
        {
            using (_unitOfWork) 
            { 
                Location stubLocation = new Location { LocationId = locationId };
                _unitOfWork.LocationRepository.Delete(stubLocation);
                _unitOfWork.Save();
            }
        }

        public async Task<LocationDTO> EditLocationInfo(LocationEditDTO locationInfo)
        {
            using(_unitOfWork)
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
    }
}
