using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public virtual ICollection<TeamDTO> MyTeams { get; set; }
        public virtual ICollection<ItemDTO> MyItems { get; set; }
        public virtual ICollection<TripDTO> MyTrips { get; set; }
    }
}
