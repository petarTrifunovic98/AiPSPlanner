using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
        
        public string Unit { get; set; }

        [Required]
        public bool Checked { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
