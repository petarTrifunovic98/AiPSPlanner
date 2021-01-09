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
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TripService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TripDTO> CreateTrip(int userId, TripCreateDTO newTrip)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(newTrip.From, newTrip.To))
                    return null;

                Trip trip = _mapper.Map<TripCreateDTO, Trip>(newTrip);
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                if (user.MyTrips == null)
                    user.MyTrips = new List<Trip>();
                user.MyTrips.Add(trip);

                if (trip.Travelers == null)
                    trip.Travelers = new List<User>();
                trip.Travelers.Add(user);

                await _unitOfWork.TripRepository.Create(trip);
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();

                TripDTO returnTrip = _mapper.Map<Trip, TripDTO>(trip);
                return returnTrip;
            }
        }

        public async Task<TripDTO> EditTripInfo(TripEditDTO tripInfo)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(tripInfo.From, tripInfo.To))
                    return null;

                Trip trip = await _unitOfWork.TripRepository.FindByID(tripInfo.TripId);
                trip.Name = tripInfo.Name;
                trip.Description = tripInfo.Description;
                trip.From = tripInfo.From;
                trip.To = tripInfo.To;
                _unitOfWork.TripRepository.Update(trip);
                _unitOfWork.Save();
                TripDTO returnTrip = _mapper.Map<Trip, TripDTO>(trip);
                return returnTrip;
            }
        }

        public async Task<bool> RemoveUserFromTrip(int tripId, int userId)
        {
            using (_unitOfWork)
            {
                Trip trip = await _unitOfWork.TripRepository.GetTripWithMembers(tripId);
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                if (trip.Travelers != null && trip.Travelers.Contains(user))
                {
                    trip.Travelers.Remove(user);
                    user.MyTrips.Remove(trip);

                    _unitOfWork.TripRepository.Update(trip);
                    _unitOfWork.UserRepository.Update(user);

                    if (trip.Travelers.Count == 0)
                        _unitOfWork.TripRepository.Delete(tripId);

                    _unitOfWork.Save();

                    return true;
                }

                return false;
            }
        }

        public async Task<TripDTO> AddMemberToTrip(int tripId, int memberId, bool IsTeam)
        {
            using (_unitOfWork)
            {
                Trip trip = await _unitOfWork.TripRepository.GetTripWithMembers(tripId);
                Member member;
                if (IsTeam)
                    member = await _unitOfWork.TeamRepository.GetTeamWithMembers(memberId);
                else
                    member = await _unitOfWork.UserRepository.FindByID(memberId);

                if (trip.Travelers == null)
                    trip.Travelers = new List<User>();

                foreach (User user in member.GetUsers())
                {
                    if (!trip.Travelers.Contains(user))
                    {
                        trip.Travelers.Add(user);
                        if (user.MyTrips == null)
                            user.MyTrips = new List<Trip>();
                        user.MyTrips.Add(trip);
                        _unitOfWork.UserRepository.Update(user);
                    }
                }
                _unitOfWork.TripRepository.Update(trip);
                _unitOfWork.Save();

                TripDTO retTrip = _mapper.Map<Trip, TripDTO>(trip);
                return retTrip;
            }
        }
    }
}
