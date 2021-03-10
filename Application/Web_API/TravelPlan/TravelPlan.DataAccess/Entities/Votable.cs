using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class Votable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VotableId { get; set; }

        public int PositiveVotes { get; set; }
        
        public int NegativeVotes { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
