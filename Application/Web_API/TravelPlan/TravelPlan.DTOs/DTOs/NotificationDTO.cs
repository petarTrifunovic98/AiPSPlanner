using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.DTOs.DTOs
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }

        public NotificationType Type { get; set; }

        public String RelatedObjectName { get; set; }

        public bool Seen { get; set; }
    }
}
