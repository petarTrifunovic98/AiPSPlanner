using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Helpers
{
    public class RedisAppSettings
    {
        public string ConnectionString { get; set; }
        public int InitialEditRightsTTL { get; set; }
        public int EditRightsProlongedTTL { get; set; }
    }
}
