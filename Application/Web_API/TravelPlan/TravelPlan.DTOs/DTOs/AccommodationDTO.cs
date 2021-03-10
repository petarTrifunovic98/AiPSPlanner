using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.DTOs.DTOs
{
    public class AccommodationDTO
    {
        public int AccommodationId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Address { get; set; }
        public ICollection<AccommodationPictureDTO> Pictures { get; set; }
        public int LocationId { get; set; }
        public int VotableId { get; set; }
        public virtual VotableDTO Votable { get; set; }
    }
}
