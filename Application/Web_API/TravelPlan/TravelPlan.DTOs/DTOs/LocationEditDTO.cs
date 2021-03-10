using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class LocationEditDTO
    {
        public int LocationId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
