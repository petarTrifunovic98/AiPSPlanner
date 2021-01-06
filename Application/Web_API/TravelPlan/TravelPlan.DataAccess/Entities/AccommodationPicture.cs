using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TravelPlan.DataAccess.Entities
{
    public class AccommodationPicture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccommodationPictureId { get; set; }

        [Required]
        public String Picture { get; set; }

        public int AccommodationId { get; set; }
        public virtual Accommodation Accommodation { get; set; }
    }
}
