using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }

        public int TripId { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
