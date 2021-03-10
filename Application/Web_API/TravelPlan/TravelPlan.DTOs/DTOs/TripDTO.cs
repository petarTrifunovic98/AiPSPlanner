using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class TripDTO
    {
        public int TripId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual ICollection<LocationDTO> Locations { get; set; }
        public virtual ICollection<UserBasicDTO> Travelers { get; set; }
        public virtual ICollection<ItemDTO> ItemList { get; set; }
        public String Icon { get; set; }
    }
}
