using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class UserChangePassDTO
    {
        public int UserId { get; set; }
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
    }
}
