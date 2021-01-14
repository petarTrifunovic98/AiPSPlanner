using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class AddOnDTO
    {
        public int AddOnId { get; set; }
        public String Description { get; set; }
        public int Price { get; set; }
        public int VotableId { get; set; }
        public virtual VotableDTO Votable { get; set; }
        public int Lvl1DependId { get; set; }
        public int Lvl2DependId { get; set; }
        public String Type { get; set; }
    }
}
