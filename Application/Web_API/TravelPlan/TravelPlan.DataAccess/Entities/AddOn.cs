using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public abstract class AddOn
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddOnId { get; set; }
        public String Description { get; set; }
        public int Price { get; set; }
        public int VotableId { get; set; }
        public virtual Votable Votable { get; set; }

        public virtual void SetDecoratorId(int id)
        { }

        public virtual int GetDecoratorId()
        {
            return 0;
        }

        public virtual int GetLvl1DependId()
        {
            return 0;
        }

        public virtual int GetLvl2DependId()
        {
            return 0;
        }
    }
}
