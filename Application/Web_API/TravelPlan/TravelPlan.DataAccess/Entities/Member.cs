using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public interface Member
    {
        public abstract List<User> GetUsers();
    }
}
