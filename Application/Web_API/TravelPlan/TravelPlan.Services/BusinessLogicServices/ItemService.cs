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
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemDTO> CreateItem(ItemCreateDTO newItem)
        {
            using (_unitOfWork)
            {
                Item item = _mapper.Map<ItemCreateDTO, Item>(newItem);
                User user = await _unitOfWork.UserRepository.GetUserWithItems(newItem.UserId);
                Trip trip = await _unitOfWork.TripRepository.GetTripWithItemsAndMembers(newItem.TripId);

                if (!trip.Travelers.Contains(user))
                    return null;

                item.User = user;
                item.Trip = trip;

                if (user.MyItems == null)
                    user.MyItems = new List<Item>();
                user.MyItems.Add(item);

                if (trip.ItemList == null)
                    trip.ItemList = new List<Item>();
                trip.ItemList.Add(item);

                await _unitOfWork.ItemRepository.Create(item);
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.TripRepository.Update(trip);
                await _unitOfWork.Save();

                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                return retItem;
            }
        }

        public async Task<bool> DeleteItem(int itemId)
        {
            using (_unitOfWork)
            {
                Item item = await _unitOfWork.ItemRepository.FindByID(itemId);
                User user = await _unitOfWork.UserRepository.GetUserWithItems(item.UserId);
                Trip trip = await _unitOfWork.TripRepository.GetTripWithItemsAndMembers(item.TripId);

                user.MyItems.Remove(item);
                trip.ItemList.Remove(item);

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.TripRepository.Update(trip);

                _unitOfWork.ItemRepository.Delete(itemId);
                await _unitOfWork.Save();

                return true;
            }
        }

        public async Task<ItemDTO> EditItemInfo(ItemEditDTO itemInfo)
        {
            using (_unitOfWork)
            {
                Item item = await _unitOfWork.ItemRepository.FindByID(itemInfo.ItemId);
                item.Name = itemInfo.Name;
                item.Description = itemInfo.Description;
                item.Amount = itemInfo.Amount;
                item.Unit = itemInfo.Unit;
                _unitOfWork.ItemRepository.Update(item);
                await _unitOfWork.Save();
                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                return retItem;
            }
        }

        public async Task<ItemDTO> ChangeUser(int ItemId, int newUserId)
        {
            using (_unitOfWork)
            {
                Item item = await _unitOfWork.ItemRepository.FindByID(ItemId);
                User user = await _unitOfWork.UserRepository.GetUserWithItems(item.UserId);
                Trip trip = await _unitOfWork.TripRepository.GetTripWithItemsAndMembers(item.TripId);
                User newUser = await _unitOfWork.UserRepository.GetUserWithItems(newUserId);

                if (!trip.Travelers.Contains(newUser))
                    return null;

                user.MyItems.Remove(item);
                item.UserId = newUserId;
                item.User = newUser;
                if (newUser.MyItems == null)
                    newUser.MyItems = new List<Item>();
                newUser.MyItems.Add(item);
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.UserRepository.Update(newUser);
                _unitOfWork.ItemRepository.Update(item);
                await _unitOfWork.Save();

                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                return retItem;
            }
        }

        public async Task<ItemDTO> un_checkItem(int ItemId)
        {
            using (_unitOfWork)
            {
                Item item = await _unitOfWork.ItemRepository.FindByID(ItemId);
                item.Checked = !item.Checked;

                _unitOfWork.ItemRepository.Update(item);
                await _unitOfWork.Save();

                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                return retItem;
            }
        }

        public async Task<IEnumerable<ItemDTO>> GetTripItems(int tripId)
        {
            using (_unitOfWork)
            {
                IEnumerable<Item> items = await _unitOfWork.ItemRepository.GetTripItems(tripId);
                IEnumerable<ItemDTO> itemsInfos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items);
                return itemsInfos;
            }
        }

        public async Task<ItemDTO> GetSpecificItem(int itemId)
        {
            using (_unitOfWork)
            {
                Item item = await _unitOfWork.ItemRepository.GetSpecificItemWithUserAndTrip(itemId);
                return _mapper.Map<Item, ItemDTO>(item);
            }
        }

        public async Task<IEnumerable<ItemDTO>> GetUserTripItems(int userId, int tripId)
        {
            using (_unitOfWork)
            {
                IEnumerable<Item> items = await _unitOfWork.ItemRepository.GetUserTripItems(userId, tripId);
                return _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items);
            }
        }

        public async Task<IEnumerable<ItemDTO>> GetUserItems(int userId)
        {
            using (_unitOfWork)
            {
                IEnumerable<Item> items = await _unitOfWork.ItemRepository.GetUserItems(userId);
                return _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items);
            }
        }
    }
}
