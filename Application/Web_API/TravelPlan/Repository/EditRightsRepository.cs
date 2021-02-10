using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.RepositoryContracts;

namespace TravelPlan.Repository
{
    public class EditRightsRepository: IEditRightsRepository
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public EditRightsRepository(IRedisConnectionBuilder redisConnectionBuilder)
        {
            _redisConnection = redisConnectionBuilder.Connection;
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
            string nextUserId = null;
            if(values.Length > 0)
                nextUserId = values[0].ToString();
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
            RedisValue value = await redisDb.StringGetAsync($"trip:{tripId}:edit.rights.holder");
            string currentRightHolder = value.ToString();
            return currentRightHolder;
        }

        public async Task<long> RequestTripEdit(int tripId, int userId)
        {
            IDatabase redisDb = _redisConnection.GetDatabase();
            long requestsNum = await redisDb.ListRightPushAsync($"trip:{tripId}:edit.requests", userId);
            return requestsNum;
        }
    }
}
