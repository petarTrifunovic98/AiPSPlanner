using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserBasicDTO> Members { get; set; }
    }
}
