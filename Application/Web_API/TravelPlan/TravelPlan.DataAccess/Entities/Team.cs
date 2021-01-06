using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class Team : Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Members { get; set; }

        //public override List<User> GetUsers()
        //{
        //    List<User> users = new List<User>();
        //    foreach (User member in Members)
        //    {
        //        if(!Members.Contains(member))
        //            users.AddRange(member.GetUsers());
        //    }
        //    return users;
        //}
    }
}
