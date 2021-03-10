using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class TeamUserListDTO
    {
        public int TeamId { get; set; }
        public IEnumerable<UserBasicDTO> Users { get; set; }
    }
}
