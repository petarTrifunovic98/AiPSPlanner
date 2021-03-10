using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class ItemEditDTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }
    }
}
