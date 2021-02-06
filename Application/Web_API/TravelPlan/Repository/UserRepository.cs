using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(TravelPlanDbContext context) :base(context)
        { }

        public bool UsernameTaken(string username)
        {
            return _dbSet.Any(u => u.Username == username);
        }

        public async Task<User> GetUserWithItems(int id)
        {
            User user = await _dbSet.Include(user => user.MyItems).FirstOrDefaultAsync(user => user.UserId == id);
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User user = await _dbSet.FirstOrDefaultAsync(user => user.Username == username);
            return user;
        }

        public int GetUnseenNotificationNumber(int userId)
        {
            return _dbSet.Where(user => user.UserId == userId)
                         .Select(user => user.MyNotifications)
                         .FirstOrDefault()
                         .Where(notification => !notification.Seen)
                         .Count();
        }

        public async Task<ICollection<Notification>> GetNotifications(int userId)
        {
            return await _dbSet.Where(user => user.UserId == userId).Select(user => user.MyNotifications).FirstOrDefaultAsync();
        }
    }
}
