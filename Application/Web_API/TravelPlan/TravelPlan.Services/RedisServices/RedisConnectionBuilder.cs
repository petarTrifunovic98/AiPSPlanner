using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;

namespace TravelPlan.Services.RedisServices
{
    public class RedisConnectionBuilder: IRedisConnectionBuilder
    {
        private static IConnectionMultiplexer _connection = null;
        private static object _objectLock = new object();
        private static readonly string ConnectionString = "localhost";
        
        public IConnectionMultiplexer Connection
        {
            get
            {
                if(_connection == null)
                {
                    lock(_objectLock)
                    {
                        if(_connection == null)
                            _connection = ConnectionMultiplexer.Connect(ConnectionString);
                    }
                }
                return _connection;
            }
        }
    }
}
