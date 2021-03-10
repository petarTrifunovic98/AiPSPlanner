using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class VotableDTO
    {
        public int VotableId { get; set; }
        public int PositiveVotes { get; set; }
        public int NegativeVotes { get; set; }
    }
}
