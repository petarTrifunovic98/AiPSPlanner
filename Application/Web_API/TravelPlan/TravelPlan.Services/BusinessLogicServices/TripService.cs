using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Helpers;
using TravelPlan.Services.BusinessLogicServices.AbstractFactoryServices;
using TravelPlan.Services.MessagingService;
using System.Linq;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private MessageControllerService _messageControllerService;

        private static List<String> tripTypes;
        private static object _lock = new object();

        public TripService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<MessageHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageControllerService = new MessageControllerService(hubContext);
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

                AbstractFactory factory = getConcreteFactory(newTrip.TripCategory);

                trip.AddOn = factory.CreateAddOn();
                trip.TripType = factory.CreateTripType();

                await _unitOfWork.TripTypeRepository.Create(trip.TripType);
                await _unitOfWork.TripRepository.Create(trip);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Save();

                TripDTO returnTrip = _mapper.Map<Trip, TripDTO>(trip);
                return returnTrip;
            }
        }

        public AbstractFactory getConcreteFactory(TripCategory tripCategory)
        {
            AbstractFactory factory;
            if (tripCategory == TripCategory.Sea)
                factory = new SeaFactory();
            else if (tripCategory == TripCategory.Winter)
                factory = new WinterFactory();
            else if (tripCategory == TripCategory.Spa)
                factory = new SpaFactory();
            else
                factory = new OtherFactory();
            return factory;
        }

        public async Task<TripDTO> EditTripInfo(TripEditDTO tripInfo)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(tripInfo.From, tripInfo.To))
                    return null;

                Trip trip = await _unitOfWork.TripRepository.GetTripWithILocations(tripInfo.TripId);
                if(trip.Locations != null)
                {
                    foreach (Location location in trip.Locations)
                    {
                        if (!DateManagerService.checkParentChildDates(tripInfo.From, tripInfo.To, location.From, location.To))
                            return null;
                    }
                }

                trip.Name = tripInfo.Name;
                trip.Description = tripInfo.Description;
                trip.From = tripInfo.From;
                trip.To = tripInfo.To;
                _unitOfWork.TripRepository.Update(trip);
                await _unitOfWork.Save();
                TripDTO returnTrip = _mapper.Map<Trip, TripDTO>(trip);
                TripBasicDTO notificationTrip = _mapper.Map<Trip, TripBasicDTO>(trip);
                await _messageControllerService.NotifyOnTripChanges(trip.TripId, "EditTripInfo", notificationTrip);
                return returnTrip;
            }
        }

        public async Task<bool> RemoveUserFromTrip(int tripId, int userId)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.GetUserWithItems(userId);
                ICollection<Item> userItems = user.MyItems.Where(item => item.TripId == tripId).ToList();
                if (userItems.Count > 0)
                    return false;

                Trip trip = await _unitOfWork.TripRepository.GetTripWithMembersAndLocations(tripId);
                if (trip.Travelers != null && trip.Travelers.Contains(user))
                {
                    trip.Travelers.Remove(user);
                    user.MyTrips.Remove(trip);

                    _unitOfWork.TripRepository.Update(trip);
                    _unitOfWork.UserRepository.Update(user);

                    if (trip.Travelers.Count == 0)
                    {
                        foreach(Location location in trip.Locations)
                        {
                            Location locationFull = await _unitOfWork.LocationRepository.GetLocationWithAccommodations(location.LocationId);
                            foreach (Accommodation accommodation in locationFull.Accommodations)
                            {
                                _unitOfWork.VotableRepository.Delete(accommodation.VotableId);
                                _unitOfWork.AccommodationRepository.Delete(accommodation.AccommodationId);
                            }
                            _unitOfWork.VotableRepository.Delete(location.VotableId);
                        }

                        AddOn addOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(trip.AddOnId);
                        while(addOn.GetDecoratorId() != 0)
                        {
                            trip.AddOnId = addOn.GetDecoratorId();
                            _unitOfWork.TripRepository.Update(trip);
                            _unitOfWork.AddOnRepository.Delete(addOn.AddOnId);
                            _unitOfWork.VotableRepository.Delete(addOn.VotableId);
                            addOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(trip.AddOnId);
                        }

                        _unitOfWork.TripRepository.Delete(tripId);
                        _unitOfWork.AddOnRepository.Delete(addOn.AddOnId);
                        _unitOfWork.VotableRepository.Delete(addOn.VotableId);
                    }

                    await _unitOfWork.Save();

                    await _messageControllerService.NotifyOnTripChanges(tripId, "RemoveUserFromTrip", userId);

                    return true;
                }

                return false;
            }
        }

        public async Task<TripDTO> AddMemberToTrip(int tripId, int memberId, bool IsTeam)
        {
            using (_unitOfWork)
            {
                Trip trip = await _unitOfWork.TripRepository.GetTripWithItemsAndMembers(tripId);
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
                        Notification notification = new Notification()
                        {
                            Seen = false,
                            RelatedObjectName = trip.Name,
                            Type = NotificationType.NewTrip,
                            User = user,
                            UserId = user.UserId
                        };
                        await _unitOfWork.NotificationRepository.Create(notification);
                        NotificationTripDTO notificationTrip = new NotificationTripDTO()
                        {
                            Notification = _mapper.Map<Notification, NotificationDTO>(notification),
                            Trip = _mapper.Map<Trip, TripDTO>(trip)
                        };
                        await _messageControllerService.SendNotification(user.UserId, "AddToTripNotification", notificationTrip);
                    }
                }
                _unitOfWork.TripRepository.Update(trip);
                await _unitOfWork.Save();

                await _messageControllerService.NotifyOnTripChanges(trip.TripId, "AddMemberToTrip", 
                    member.GetUsers().Select(user => _mapper.Map<User, UserBasicDTO>(user)));

                TripDTO retTrip = _mapper.Map<Trip, TripDTO>(trip);
                return retTrip;
            }
        }

        public async Task<TripDTO> GetTripWithItemsAndMembers(int tripId)
        {
            using (_unitOfWork)
            {
                Trip trip = await _unitOfWork.TripRepository.GetTripWithItemsAndMembers(tripId);
                return _mapper.Map<Trip, TripDTO>(trip);
            }
        }

        public async Task<IEnumerable<TripDTO>> GetUserTrips(int userId)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                IEnumerable<Trip> trips = await _unitOfWork.TripRepository.GetUserTrips(user);
                IEnumerable<TripDTO> tripsInfos = _mapper.Map<IEnumerable<Trip>, IEnumerable<TripDTO>>(trips);
                return tripsInfos;
            }
        }

        public async Task<TripDTO> GetTripWithLocations(int tripId)
        {
            using (_unitOfWork)
            {
                Trip trip = await _unitOfWork.TripRepository.GetTripWithILocations(tripId);
                return _mapper.Map<Trip, TripDTO>(trip);
            }
        }

        public async Task<TripAdditionalInfoDTO> GetTripAdditionalInfo(int tripId)
        {
            using(_unitOfWork)
            {
                TripType tripType = await _unitOfWork.TripRepository.GetTripTripType(tripId);
                return _mapper.Map<TripType, TripAdditionalInfoDTO>(tripType);
            }
        }

        public async Task<TripAdditionalInfoDTO> AddItemToPackingList(int tripId, TripStandardListItemDTO item)
        {
            using (_unitOfWork)
            {
                TripType tripType = await _unitOfWork.TripRepository.GetTripTripType(tripId);
                tripType.StandardList += "_" + item.item;
                _unitOfWork.TripTypeRepository.Update(tripType);
                await _unitOfWork.Save();
                return _mapper.Map<TripType, TripAdditionalInfoDTO>(tripType);
            }
        }

        public List<string> GetTripTypes()
        {
            if (tripTypes == null)
            {
                lock (_lock)
                {
                    tripTypes = new List<string>();
                    tripTypes.Add("Sea");
                    tripTypes.Add("Winter");
                    tripTypes.Add("Spa");
                    tripTypes.Add("Other");
                }
            }
            return tripTypes;
        }
    }
}
