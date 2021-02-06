using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Contracts
{
    public interface IRedisConnectionBuilder
    {
        IConnectionMultiplexer Connection { get; }
    }
}
