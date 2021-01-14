using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class WinterDecorator : WinterAddOn
    {
        [ForeignKey("Decorator")]
        public int DecoratorId { get; set; }
        public WinterAddOn Decorator { get; set; }

        public override void SetDecoratorId(int id)
        {
            this.DecoratorId = id;
        }

        public override int GetDecoratorId()
        {
            return this.DecoratorId;
        }
    }
}
