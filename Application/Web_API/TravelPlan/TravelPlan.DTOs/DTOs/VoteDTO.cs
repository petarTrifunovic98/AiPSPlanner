using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class VoteDTO
    {
        public int VoteId { get; set; }
        public bool Positive { get; set; }
        public int UserId { get; set; }
        public UserBasicDTO User { get; set; }
        public int VotableId { get; set; }
        public VotableDTO Votable { get; set; }
    }
}
