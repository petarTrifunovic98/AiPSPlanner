using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.Services.AuthentificationService
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int SaltLength { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
