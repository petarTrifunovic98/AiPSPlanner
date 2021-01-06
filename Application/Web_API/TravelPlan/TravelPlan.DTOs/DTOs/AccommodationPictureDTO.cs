using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class AccommodationPictureDTO
    {
        public int AccommodationPictureId { get; set; }
        public string Picture { get; set; }
        public int AccommodationId { get; set; }
        public AccommodationDTO Accommodation { get;set;}
    }
}
