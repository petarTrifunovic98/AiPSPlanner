using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class ItemDTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }
        public bool Checked { get; set; }
        public int UserId { get; set; }
        public virtual UserDTO User { get; set; }
        public int TripId { get; set; }
        public virtual TripDTO Trip { get; set; }
    }
}
