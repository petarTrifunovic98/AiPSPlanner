using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class AddOnService : IAddOnService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private static List<DecorationAvailableDTO> SeaList;
        private static List<DecorationAvailableDTO> WinterList;
        private static List<DecorationAvailableDTO> SpaList;

        public AddOnService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            return SeaList;
        }

        private List<DecorationAvailableDTO> GetWinterDecorations()
        {
            if (WinterList == null)
            {
                WinterList = new List<DecorationAvailableDTO>();
            }
            return WinterList;
        }

        private List<DecorationAvailableDTO> GetSpaDecorations()
        {
            if (SpaList == null)
            {
                SpaList = new List<DecorationAvailableDTO>();
            }
            return SpaList;
        }

        public async Task<AddOnDTO> EditAddOn(AddOnEditDTO addOnInfo)
        {
            using(_unitOfWork)
            {
                AddOn addOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(addOnInfo.AddOnId);
                addOn.Price = addOnInfo.Price;
                addOn.Description = addOnInfo.Description;
                _unitOfWork.AddOnRepository.Update(addOn);
                await _unitOfWork.Save();
                return _mapper.Map<AddOn, AddOnDTO>(addOn);
            }
        }

        public async Task<AddOnDTO> CreateAddOn(AddOnCreateDTO newAddOn)
        {
            using(_unitOfWork)
            {
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
                    return _mapper.Map<AddOn, AddOnDTO>(addOn);
                }

                AddOn currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(trip.AddOnId);
                while(currAddOn.GetDecoratorId() != idToFind)
                {
                    currAddOn = await _unitOfWork.AddOnRepository.GetAddOnWithVotable(currAddOn.GetDecoratorId());
                }
                currAddOn.SetDecoratorId(addOn.AddOnId);
                _unitOfWork.AddOnRepository.Update(currAddOn);
                await _unitOfWork.Save();
                return _mapper.Map<AddOn, AddOnDTO>(addOn);
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
                default: return null;
            }
        }
    }
}
