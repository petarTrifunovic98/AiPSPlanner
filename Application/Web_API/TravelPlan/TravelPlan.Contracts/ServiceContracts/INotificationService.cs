using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface INotificationService
    {
        Task<NotificationDTO> GetNotification(int id);
        Task<ICollection<NotificationDTO>> GetNotifications(int userId);
        Task<bool> SeenMyNotifications(int userId);
        Task<bool> SeenANotification(int notificationId);
        Task<bool> DeleteSeenNotifications(int userId);
    }
}
