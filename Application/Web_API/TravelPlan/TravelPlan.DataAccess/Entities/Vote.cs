using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class Vote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }

        public bool Positive { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int VotableId { get; set; }
        public Votable Votable { get; set; }
    }
}
