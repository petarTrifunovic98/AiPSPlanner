using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class LocationCreateDTO
    {
        public string Name { get; set; }
        public String Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TripId { get; set; }
    }
}
