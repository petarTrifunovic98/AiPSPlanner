﻿using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.DTOs.DTOs
{
    public class AccommodationCreateDTO
    {
        public AccommodationType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Address { get; set; }
        public int LocationId { get; set; }
    }
}
