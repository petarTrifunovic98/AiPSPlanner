﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class User: Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Picture { get; set; }

        public virtual ICollection<Team> MyTeams { get; set; }

        public virtual ICollection<Item> MyItems { get; set; }

        public virtual ICollection<Trip> MyTrips { get; set; }

        public virtual ICollection<Notification> MyNotifications { get; set; }

        public List<User> GetUsers()
        {
            List<User> ret = new List<User>();
            ret.Add(this);
            return ret;
        }
    }
}
