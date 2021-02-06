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
        private readonly IConnectionMultiplexer _redisConnection;

        public UserRepository(TravelPlanDbContext context, IRedisConnectionBuilder redisConnectionBuilder) :base(context)
        {
            _redisConnection = redisConnectionBuilder.Connection;
        }
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

        public async Task<long> RequestTripEdit(int tripId, int userId)
        {
            IDatabase redisDb = _redisConnection.GetDatabase();
            long requestsNum = await redisDb.ListRightPushAsync($"trip:{tripId}:edit.requests", userId);
            return requestsNum;
        }

        public async Task SetEditRightHolder(int tripId, int userId)
        {
            IDatabase redisDb = _redisConnection.GetDatabase();
            await redisDb.StringSetAsync($"trip:{tripId}:edit.rights.holder", userId);
        }

        public async Task<string> GetNextRightHolder(int tripId)
        {
            IDatabase redisDb = _redisConnection.GetDatabase();
            await redisDb.ListLeftPopAsync($"trip:{tripId}:edit.requests");
            RedisValue[] values = await redisDb.ListRangeAsync($"trip:{tripId}:edit.requests", 0, 0);
            string nextUserId = values[0].ToString();
            return nextUserId;
        }

        public async Task RemoveUserFromRequestQueue(int tripId, int userId)
        {
            IDatabase redisDb = _redisConnection.GetDatabase();
            await redisDb.ListRemoveAsync($"trip:{tripId}:edit.requests", userId);
        }

        public async Task<string> GetCurrentRightHolder(int tripId)
        {
            IDatabase redisDb = _redisConnection.GetDatabase();
            RedisValue value =  await redisDb.StringGetAsync($"trip:{tripId}:edit.rights.holder");
            string currentRightHolder = value.ToString();
            return currentRightHolder;
        }
    }
}
