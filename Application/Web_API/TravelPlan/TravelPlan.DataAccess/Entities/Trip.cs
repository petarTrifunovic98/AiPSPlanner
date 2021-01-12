using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class Trip
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TripId { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public virtual ICollection<User> Travelers { get; set; }

        public virtual ICollection<Item> ItemList { get; set; }

        public int AddOnId { get; set; }
        public AddOn AddOn { get; set; }

        public int TripTypeId { get; set; }
        public TripType TripType { get; set; }
    }
}
