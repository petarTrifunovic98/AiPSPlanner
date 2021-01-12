using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace TravelPlan.DataAccess.Entities
{
    public abstract class TripType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TripTypeId { get; set; }
        public String StandardList { get; set; }

        public abstract List<String> getTipsAndTricks();
        public abstract String getIcon();

        public List<String> GetPackingList()
        {
            return StandardList.Split('_').ToList();
        }
    }
}
