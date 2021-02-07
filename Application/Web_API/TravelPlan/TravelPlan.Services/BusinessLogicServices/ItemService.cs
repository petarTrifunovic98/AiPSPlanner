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
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private MessageControllerService _messageControllerService;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<MessageHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageControllerService = new MessageControllerService(hubContext);
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

                Notification notification = new Notification();
                notification.Seen = false;
                notification.RelatedObjectName = item.Name;
                notification.Type = NotificationType.AddedItem;
                notification.User = user;
                notification.UserId = user.UserId;
                await _unitOfWork.NotificationRepository.Create(notification);

                await _unitOfWork.Save();

                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                await _messageControllerService.NotifyOnTripChanges(newItem.TripId, "AddItem", retItem);
                NotificationItemDTO notificationItem = new NotificationItemDTO()
                {
                    Notification = _mapper.Map<Notification, NotificationDTO>(notification),
                    Item = retItem
                };
                await _messageControllerService.SendNotification(newItem.UserId, "AddItemNotification", notificationItem);
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

                Notification notification = new Notification();
                notification.Seen = false;
                notification.RelatedObjectName = item.Name;
                notification.Type = NotificationType.RemovedItem;
                notification.User = user;
                notification.UserId = user.UserId;
                await _unitOfWork.NotificationRepository.Create(notification);

                await _unitOfWork.Save();

                await _messageControllerService.NotifyOnTripChanges(trip.TripId, "RemoveItem", item.ItemId);
                NotificationItemDeleteDTO notificationItemDelete = new NotificationItemDeleteDTO()
                {
                    Notification = _mapper.Map<Notification, NotificationDTO>(notification),
                    ItemToDelete = item.ItemId
                };
                await _messageControllerService.SendNotification(user.UserId, "RemoveItemNotification", notificationItemDelete);
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

                Notification notification = new Notification();
                notification.Seen = false;
                notification.RelatedObjectName = item.Name;
                notification.Type = NotificationType.EditedItem;
                notification.UserId = item.UserId;
                await _unitOfWork.NotificationRepository.Create(notification);

                await _unitOfWork.Save();
                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                await _messageControllerService.NotifyOnTripChanges(item.TripId, "EditItem", retItem);
                NotificationItemDTO notificationItem = new NotificationItemDTO()
                {
                    Notification = _mapper.Map<Notification, NotificationDTO>(notification),
                    Item = retItem
                };
                await _messageControllerService.SendNotification(item.UserId, "EditItemNotification", notificationItem);
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

                Notification notification_old = new Notification();
                notification_old.Seen = false;
                notification_old.RelatedObjectName = item.Name;
                notification_old.Type = NotificationType.RemovedItem;
                notification_old.UserId = user.UserId;
                await _unitOfWork.NotificationRepository.Create(notification_old);

                Notification notification_new = new Notification();
                notification_new.Seen = false;
                notification_new.RelatedObjectName = item.Name;
                notification_new.Type = NotificationType.AddedItem;
                notification_new.UserId = newUserId;
                await _unitOfWork.NotificationRepository.Create(notification_new);

                await _unitOfWork.Save();

                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
               
                await _messageControllerService.NotifyOnTripChanges(trip.TripId, "EditItem", retItem);
                NotificationItemDeleteDTO notificationItemDelete = new NotificationItemDeleteDTO()
                {
                    Notification = _mapper.Map<Notification, NotificationDTO>(notification_old),
                    ItemToDelete = item.ItemId
                };
                await _messageControllerService.SendNotification(user.UserId, "RemoveItemNotification", notificationItemDelete);
                NotificationItemDTO notificationItem = new NotificationItemDTO()
                {
                    Notification = _mapper.Map<Notification, NotificationDTO>(notification_new),
                    Item = retItem
                };
                await _messageControllerService.SendNotification(newUserId, "AddItemNotification", notificationItem);
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

                Notification notification = new Notification();
                notification.Seen = false;
                notification.RelatedObjectName = item.Name;
                notification.Type = NotificationType.EditedItem;
                notification.UserId = item.UserId;
                await _unitOfWork.NotificationRepository.Create(notification);

                await _unitOfWork.Save();

                ItemDTO retItem = _mapper.Map<Item, ItemDTO>(item);
                await _messageControllerService.NotifyOnTripChanges(item.TripId, "AddItem", retItem);
                NotificationItemDTO notificationItem = new NotificationItemDTO()
                {
                    Notification = _mapper.Map<Notification, NotificationDTO>(notification),
                    Item = retItem
                };
                await _messageControllerService.SendNotification(item.UserId, "AddItemNotification", notificationItem);
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
