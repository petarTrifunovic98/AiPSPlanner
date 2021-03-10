using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace TravelPlan.DataAccess.Entities
{
    public class Team : Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Members { get; set; }

        public List<User> GetUsers()
        {
            if (Members == null)
                return new List<User>();
            return Members.ToList();
        }
    }
}
