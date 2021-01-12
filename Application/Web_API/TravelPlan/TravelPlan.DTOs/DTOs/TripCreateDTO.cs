using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public enum TripCategory
    {
        Sea,
        Winter,
        Spa
    }

    public class TripCreateDTO
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public TripCategory TripCategory {get; set;}
    }
}
