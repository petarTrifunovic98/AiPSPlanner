using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public enum NotificationType
    {
        Removed,
        Added,
        Edited
    }

    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        public NotificationType Type { get; set; }

        public String ItemName { get; set; }

        public bool Seen { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
