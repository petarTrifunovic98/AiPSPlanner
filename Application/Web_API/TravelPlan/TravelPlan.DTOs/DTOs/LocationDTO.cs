using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public ICollection<AccommodationDTO> Accommodations { get; set; }
        public int TripId { get; set; }
        public virtual TripBasicDTO Trip { get; set; }
        public int VotableId { get; set; }
        public virtual VotableDTO Votable { get; set; }
    }
}
