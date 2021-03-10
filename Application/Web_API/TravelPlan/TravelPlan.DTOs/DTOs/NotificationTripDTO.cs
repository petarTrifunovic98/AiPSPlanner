using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class NotificationTripDTO
    {
        public NotificationDTO Notification { get; set; }
        public TripDTO Trip { get; set; }
    }
}
