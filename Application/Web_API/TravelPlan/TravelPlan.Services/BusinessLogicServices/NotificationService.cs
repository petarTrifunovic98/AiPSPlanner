using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using System.Linq;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NotificationDTO> GetNotification(int id)
        {
            using(_unitOfWork)
            {
                return _mapper.Map<Notification, NotificationDTO>(await _unitOfWork.NotificationRepository.FindByID(id));
            }
        }

        public async Task<ICollection<NotificationDTO>> GetNotifications(int userId)
        {
            using(_unitOfWork)
            {
                return (await _unitOfWork.UserRepository.GetNotifications(userId))
                        .Select(notification => _mapper.Map<Notification, NotificationDTO>(notification)).ToList();
            }
        }

        public async Task<bool> SeenMyNotifications(int userId)
        {
            using(_unitOfWork)
            {
                ICollection<Notification> notifications = await _unitOfWork.UserRepository.GetNotifications(userId);
                foreach(Notification notification in notifications)
                {
                    notification.Seen = true;
                    _unitOfWork.NotificationRepository.Update(notification);
                }

                await _unitOfWork.Save();
                return true;
            }
        }

        public async Task<bool> SeenANotification(int notificationId)
        {
            using(_unitOfWork)
            {
                Notification notification = await _unitOfWork.NotificationRepository.FindByID(notificationId);
                notification.Seen = true;
                _unitOfWork.NotificationRepository.Update(notification);
                await _unitOfWork.Save();
                return true;
            }
        }

        public async Task<bool> DeleteSeenNotifications(int userId)
        {
            ICollection<Notification> notifications = await _unitOfWork.UserRepository.GetNotifications(userId);
            foreach(Notification notification in notifications)
            {
                if(notification.Seen)
                    _unitOfWork.NotificationRepository.Delete(notification.NotificationId);
            }
            await _unitOfWork.Save();
            return true;
        }
    }
}
