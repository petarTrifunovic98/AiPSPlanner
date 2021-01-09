using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class VoteCreateDTO
    {
        public bool Positive { get; set; }
        public int UserId { get; set; }
        public int VotableId { get; set; }
    }
}
