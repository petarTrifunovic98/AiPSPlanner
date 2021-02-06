using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IUserRepository: IRepositoryBase<User>
    {
        bool UsernameTaken(string username);
        Task<User> GetUserWithItems(int id);
        Task<User> GetUserByUsername(string username);
        int GetUnseenNotificationNumber(int userId);
        Task<ICollection<Notification>> GetNotifications(int userId);
        Task<long> RequestTripEdit(int tripId, int userId);
        Task SetEditRightHolder(int tripId, int userId);
        Task<string> GetNextRightHolder(int tripId);
        Task<string> GetCurrentRightHolder(int tripId);
        Task RemoveUserFromRequestQueue(int tripId, int userId);
    }
}
