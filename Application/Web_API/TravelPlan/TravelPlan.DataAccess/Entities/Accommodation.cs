using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public enum AccommodationType
    {
        Other,
        Hotel,
        Room,
        Apartment,
        Household,
        Camping
    }

    public class Accommodation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccommodationId { get; set; }

        public AccommodationType Type { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        public String Address { get; set; }

        public virtual ICollection<AccommodationPicture> Pictires {get; set;}

        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
    }
}
