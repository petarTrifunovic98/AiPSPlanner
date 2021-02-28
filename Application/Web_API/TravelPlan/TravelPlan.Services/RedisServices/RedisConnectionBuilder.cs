using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.Helpers;
using TravelPlan.Services.MessagingService;

namespace TravelPlan.Services.RedisServices
{
    public class RedisConnectionBuilder: IRedisConnectionBuilder
    {
        private static IConnectionMultiplexer _connection = null;
        private static object _objectLock = new object();
        private MessageControllerService _messageControllerService;
        private readonly RedisAppSettings _redisAppSettings;

        public RedisConnectionBuilder(IHubContext<MessageHub> hubContext, IOptions<RedisAppSettings> redisAppSettings)
        {
            _messageControllerService = new MessageControllerService(hubContext);
            _redisAppSettings = redisAppSettings.Value;
        }

        public IConnectionMultiplexer Connection
        {
            get
            {
                if(_connection == null)
                {
                    lock(_objectLock)
                    {
                        if(_connection == null)
                        { 
                            _connection = ConnectionMultiplexer.Connect(_redisAppSettings.ConnectionString);
                            var redisPubSub = _connection.GetSubscriber();

                            redisPubSub.Subscribe("__keyevent@0__:expired").OnMessage(async message =>
                            {
                                string keyName = message.Message;
                                string[] keyNameParts = keyName.Split(':');
                                int tripId = int.Parse(keyNameParts[1]);

                                IDatabase redisDb = _connection.GetDatabase();
                                string previousUserId = await redisDb.ListLeftPopAsync($"trip:{tripId}:edit.requests");
                                RedisValue[] values = await redisDb.ListRangeAsync($"trip:{tripId}:edit.requests", 0, 0);
                                string nextUserId = null;
                                if (values.Length > 0)
                                    nextUserId = values[0].ToString();

                                await _messageControllerService.SendNotification(int.Parse(previousUserId), "LostEditRightsNotification", tripId);
                                if (!string.IsNullOrEmpty(nextUserId))
                                {
                                    redisDb.StringSet($"trip:{tripId}:edit.rights.holder", nextUserId);
                                    await _messageControllerService.SendNotification(int.Parse(nextUserId), "EditRightsNotification", tripId);
                                    await redisDb.KeyExpireAsync($"trip:{tripId}:edit.rights.holder", new TimeSpan(0, _redisAppSettings.InitialEditRightsTTL, 0));
                                }
                            });
                        }
                    }
                }
                return _connection;
            }
        }
    }
}
