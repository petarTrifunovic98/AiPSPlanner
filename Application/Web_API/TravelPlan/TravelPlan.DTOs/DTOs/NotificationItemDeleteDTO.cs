using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class NotificationItemDeleteDTO
    {
        public NotificationDTO Notification { get; set; }
        public int ItemToDelete { get; set; }
    }
}
