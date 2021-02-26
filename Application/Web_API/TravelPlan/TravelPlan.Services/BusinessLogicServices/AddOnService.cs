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
using TravelPlan.Services.MessagingService;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class AddOnService : IAddOnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private MessageControllerService _messageControllerService;

        private static List<DecorationAvailableDTO> SeaList;
        private static List<DecorationAvailableDTO> WinterList;
        private static List<DecorationAvailableDTO> SpaList;

        private static object _lock = new object();

        public AddOnService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<MessageHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageControllerService = new MessageControllerService(hubContext);
        }

        public async Task<List<DecorationAvailableDTO>> GetAvailableDecorations(int tripId)
        {
            using(_unitOfWork)
            {
                TripType tripType = await _unitOfWork.TripRepository.GetTripTripType(tripId);
                if (tripType.GetType() == typeof(SeaType))
                    return GetSeaDecorations();
                else if (tripType.GetType() == typeof(WinterType))
                    return GetWinterDecorations();
                else if (tripType.GetType() == typeof(SpaType))
                    return GetSpaDecorations();
                else
                    return new List<DecorationAvailableDTO>();
            }
        }

        private List<DecorationAvailableDTO> GetSeaDecorations()
        {
            if (SeaList == null)
            {
                lock (_lock)
                {
                    DecorationAvailableDTO tea = new DecorationAvailableDTO(AddOnType.Tea.ToString(), true, true);
                    DecorationAvailableDTO coffee = new DecorationAvailableDTO(AddOnType.Coffee.ToString(), true, true);
                    DecorationAvailableDTO juice = new DecorationAvailableDTO(AddOnType.Juice.ToString(), true, true);
                    DecorationAvailableDTO breakfast = new DecorationAvailableDTO(AddOnType.Breakfast.ToString(), true, false);
                    breakfast.NextLvlDecorations.Add(tea);
                    breakfast.NextLvlDecorations.Add(coffee);
                    breakfast.NextLvlDecorations.Add(juice);
                    DecorationAvailableDTO wine = new DecorationAvailableDTO(AddOnType.Wine.ToString(), true, true);
                    DecorationAvailableDTO dessert = new DecorationAvailableDTO(AddOnType.Dessert.ToString(), true, true);
                    DecorationAvailableDTO lunch = new DecorationAvailableDTO(AddOnType.Lunch.ToString(), true, false);
                    lunch.NextLvlDecorations.Add(wine);
                    lunch.NextLvlDecorations.Add(dessert);
                    DecorationAvailableDTO cruise = new DecorationAvailableDTO(AddOnType.Cruise.ToString(), false, false);
                    cruise.NextLvlDecorations.Add(breakfast);
                    cruise.NextLvlDecorations.Add(lunch);
                    SeaList = new List<DecorationAvailableDTO>
                    {
                        cruise,
                        new DecorationAvailableDTO(AddOnType.Aquapark.ToString(), false, false),
                        new DecorationAvailableDTO(AddOnType.Waterboard.ToString(), false, false),
                        new DecorationAvailableDTO(AddOnType.Sunbeds.ToString(), false, false)
                    };
                }
            }
            return SeaList;
        }

        private List<DecorationAvailableDTO> GetWinterDecorations()
        {
            if (WinterList == null)
            {
                DecorationAvailableDTO snowboard = new DecorationAvailableDTO(AddOnType.Snowboard.ToString(), true, false);
                DecorationAvailableDTO skis = new DecorationAvailableDTO(AddOnType.Skis.ToString(), true, false);
                DecorationAvailableDTO skiPoles = new DecorationAvailableDTO(AddOnType.SkiPoles.ToString(), true, false);
                DecorationAvailableDTO skiBoots = new DecorationAvailableDTO(AddOnType.SkiBoots.ToString(), true, false);
                DecorationAvailableDTO sled = new DecorationAvailableDTO(AddOnType.Sled.ToString(), true, false);
                DecorationAvailableDTO skiEquipment = new DecorationAvailableDTO(AddOnType.SkiEquipment.ToString(), false, false);
                skiEquipment.NextLvlDecorations.Add(snowboard);
                skiEquipment.NextLvlDecorations.Add(skis);
                skiEquipment.NextLvlDecorations.Add(skiPoles);
                skiEquipment.NextLvlDecorations.Add(skiBoots);
                skiEquipment.NextLvlDecorations.Add(sled);
                WinterList = new List<DecorationAvailableDTO>
                {
                    skiEquipment,
                    new DecorationAvailableDTO(AddOnType.SkiPass.ToString(), false, false)
                };
            }
            return WinterList;
        }

        private List<DecorationAvailableDTO> GetSpaDecorations()
        {
            if (SpaList == null)
            {
                DecorationAvailableDTO schnapps = new DecorationAvailableDTO(AddOnType.Schnapps.ToString(), true, true);
                DecorationAvailableDTO pogaca = new DecorationAvailableDTO(AddOnType.Pogaca.ToString(), true, true);
                DecorationAvailableDTO meal = new DecorationAvailableDTO(AddOnType.Meal.ToString(), true, false);
                meal.NextLvlDecorations.Add(schnapps);
                meal.NextLvlDecorations.Add(pogaca);
                DecorationAvailableDTO trainTour = new DecorationAvailableDTO(AddOnType.TrainTour.ToString(), false, false);
                trainTour.NextLvlDecorations.Add(meal);
                DecorationAvailableDTO tourGuide = new DecorationAvailableDTO(AddOnType.TourGuide.ToString(), true, false);
                DecorationAvailableDTO walk = new DecorationAvailableDTO(AddOnType.Walk.ToString(), false, false);
                walk.NextLvlDecorations.Add(tourGuide);
                SpaList = new List<DecorationAvailableDTO>
                {
                    trainTour,
                    walk,
                    new DecorationAvailableDTO(AddOnType.BikeRent.ToString(), false, false),
                    new DecorationAvailableDTO(AddOnType.ScooterRent.ToString(), false, false)
                };
            }
            return SpaList;
        }

        public async Task<AddOnDTO> EditAddOn(AddOnEditDTO addOnInfo, int tripId)
        {
            using(_unitOfWork)
            {
                AddOn addOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(addOnInfo.AddOnId);
                addOn.Price = addOnInfo.Price;
                addOn.Description = addOnInfo.Description;
                _unitOfWork.AddOnRepository.Update(addOn);
                await _unitOfWork.Save();
                AddOnDTO retValue = _mapper.Map<AddOn, AddOnDTO>(addOn);
                await _messageControllerService.NotifyOnTripChanges(tripId, "EditAddOn", retValue);
                return retValue;
            }
        }

        public async Task<AddOnDTO> CreateAddOn(AddOnCreateDTO newAddOn)
        {
            using(_unitOfWork)
            {
                AddOnDTO retValue;
                AddOn addOn = MapNewAddOn(newAddOn);
                addOn.Votable = new Votable();
                int idToFind = 0;
                if (newAddOn.Lvl2DependId != 0)
                    idToFind = newAddOn.Lvl2DependId;
                else if (newAddOn.Lvl1DependId != 0)
                    idToFind = newAddOn.Lvl1DependId;

                Trip trip = await _unitOfWork.TripRepository.FindByID(newAddOn.TripId);
                if (idToFind == 0)
                    idToFind = trip.AddOnId;

                addOn.SetDecoratorId(idToFind);
                await _unitOfWork.AddOnRepository.Create(addOn);
                await _unitOfWork.Save();

                if (trip.AddOnId == idToFind)
                {
                    trip.AddOnId = addOn.AddOnId;
                    _unitOfWork.TripRepository.Update(trip);
                    await _unitOfWork.Save();
                    retValue =  _mapper.Map<AddOn, AddOnDTO>(addOn);
                    await _messageControllerService.NotifyOnTripChanges(newAddOn.TripId, "AddAddOn", retValue);
                    return retValue;
                }

                AddOn currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(trip.AddOnId);
                while(currAddOn.GetDecoratorId() != idToFind)
                {
                    currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(currAddOn.GetDecoratorId());
                }
                currAddOn.SetDecoratorId(addOn.AddOnId);
                _unitOfWork.AddOnRepository.Update(currAddOn);
                await _unitOfWork.Save();
                retValue = _mapper.Map<AddOn, AddOnDTO>(addOn);
                await _messageControllerService.NotifyOnTripChanges(newAddOn.TripId, "AddAddOn", retValue);
                return retValue;
            }
        }

        private AddOn MapNewAddOn(AddOnCreateDTO newAddOn)
        {
            switch(newAddOn.AddOnType)
            {
                case (AddOnType.Cruise): return _mapper.Map<AddOnCreateDTO, Cruise>(newAddOn);
                case (AddOnType.Breakfast): return _mapper.Map<AddOnCreateDTO, Breakfast>(newAddOn);
                case (AddOnType.Lunch): return _mapper.Map<AddOnCreateDTO, Lunch>(newAddOn);
                case (AddOnType.Tea): return _mapper.Map<AddOnCreateDTO, Tea>(newAddOn);
                case (AddOnType.Coffee): return _mapper.Map<AddOnCreateDTO, Coffee>(newAddOn);
                case (AddOnType.Juice): return _mapper.Map<AddOnCreateDTO, Juice>(newAddOn);
                case (AddOnType.Wine): return _mapper.Map<AddOnCreateDTO, Wine>(newAddOn);
                case (AddOnType.Dessert): return _mapper.Map<AddOnCreateDTO, Dessert>(newAddOn);
                case (AddOnType.Aquapark): return _mapper.Map<AddOnCreateDTO, Aquapark>(newAddOn);
                case (AddOnType.Waterboard): return _mapper.Map<AddOnCreateDTO, Waterboard>(newAddOn);
                case (AddOnType.Sunbeds): return _mapper.Map<AddOnCreateDTO, SunBeds>(newAddOn);
                case (AddOnType.SkiPass): return _mapper.Map<AddOnCreateDTO, SkiPass>(newAddOn);
                case (AddOnType.SkiEquipment): return _mapper.Map<AddOnCreateDTO, SkiEquipment>(newAddOn);
                case (AddOnType.Snowboard): return _mapper.Map<AddOnCreateDTO, Snowboard>(newAddOn);
                case (AddOnType.Skis): return _mapper.Map<AddOnCreateDTO, Skis>(newAddOn);
                case (AddOnType.SkiPoles): return _mapper.Map<AddOnCreateDTO, SkiPoles>(newAddOn);
                case (AddOnType.SkiBoots): return _mapper.Map<AddOnCreateDTO, SkiBoots>(newAddOn);
                case (AddOnType.Sled): return _mapper.Map<AddOnCreateDTO, Sled>(newAddOn);
                case (AddOnType.BikeRent): return _mapper.Map<AddOnCreateDTO, BikeRent>(newAddOn);
                case (AddOnType.ScooterRent): return _mapper.Map<AddOnCreateDTO, ScooterRent>(newAddOn);
                case (AddOnType.Walk): return _mapper.Map<AddOnCreateDTO, Walk>(newAddOn);
                case (AddOnType.TourGuide): return _mapper.Map<AddOnCreateDTO, TourGuide>(newAddOn);
                case (AddOnType.TrainTour): return _mapper.Map<AddOnCreateDTO, TrainTour>(newAddOn);
                case (AddOnType.Meal): return _mapper.Map<AddOnCreateDTO, Meal>(newAddOn);
                case (AddOnType.Pogaca): return _mapper.Map<AddOnCreateDTO, Pogaca>(newAddOn);
                case (AddOnType.Schnapps): return _mapper.Map<AddOnCreateDTO, Schnapps>(newAddOn);
                default: return null;
            }
        }

        public async Task<bool> DeleteAddOn(int addOnId, int tripId)
        {
            using(_unitOfWork)
            {
                if (addOnId <= 0)
                    return false;

                AddOnDTO deletedAddOn = null;

                AddOn prevAddOn = null;
                Trip trip = await _unitOfWork.TripRepository.FindByID(tripId);
                AddOn currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(trip.AddOnId);
                while(true)
                {
                    if(currAddOn.GetDecoratorId() == 0)
                        break;

                    if(currAddOn.AddOnId == addOnId)
                    {
                        if(prevAddOn == null)
                        {
                            trip.AddOnId = currAddOn.GetDecoratorId();
                            _unitOfWork.TripRepository.Update(trip);
                        }
                        else
                        {
                            prevAddOn.SetDecoratorId(currAddOn.GetDecoratorId());
                            _unitOfWork.AddOnRepository.Update(prevAddOn);
                        }

                        _unitOfWork.AddOnRepository.Delete(currAddOn.AddOnId);
                        _unitOfWork.VotableRepository.Delete(currAddOn.VotableId);
                        await _unitOfWork.Save();
                        deletedAddOn = _mapper.Map<AddOn, AddOnDTO>(currAddOn);
                        break;
                    }

                    if (currAddOn.GetLvl1DependId() == addOnId || currAddOn.GetLvl2DependId() == addOnId)
                    {
                        if (prevAddOn == null)
                        {
                            trip.AddOnId = currAddOn.GetDecoratorId();
                            _unitOfWork.TripRepository.Update(trip);
                        }
                        else
                        {
                            prevAddOn.SetDecoratorId(currAddOn.GetDecoratorId());
                            _unitOfWork.AddOnRepository.Update(prevAddOn);
                        }
                        
                        _unitOfWork.AddOnRepository.Delete(currAddOn.AddOnId);
                        _unitOfWork.VotableRepository.Delete(currAddOn.VotableId);
                        //deletedIds.Add(currAddOn.AddOnId);
                        //deletedAddOns.Add(_mapper.Map<AddOn, AddOnDTO>(currAddOn));
                        await _unitOfWork.Save();
                    }
                    else
                        prevAddOn = currAddOn;

                    if (prevAddOn == null)
                        currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(trip.AddOnId);
                    else
                        currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(prevAddOn.GetDecoratorId());
                }
                //await _messageControllerService.NotifyOnTripChanges(tripId, "RemoveAddOn", deletedIds);
                //await _messageControllerService.NotifyOnTripChanges(tripId, "RemoveAddOn", deletedAddOns);
                await _messageControllerService.NotifyOnTripChanges(tripId, "RemoveAddOn", deletedAddOn);
                return true;
            }
        }

        public async Task<List<AddOnDTO>> GetTripAddOns(int tripId)
        {
            using(_unitOfWork)
            {
                List<AddOnDTO> retList = new List<AddOnDTO>();
                AddOn addOn = await _unitOfWork.TripRepository.GetTripFirstAddOn(tripId);
                while(addOn.GetDecoratorId() != 0)
                {
                    retList.Add(_mapper.Map<AddOn, AddOnDTO>(addOn));
                    addOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(addOn.GetDecoratorId());
                }
                return retList;
            }
        }
    }
}
