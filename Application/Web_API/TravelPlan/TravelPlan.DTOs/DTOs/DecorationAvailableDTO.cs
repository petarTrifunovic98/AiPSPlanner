using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public class DecorationAvailableDTO
    {
        public String TypeName { get; set; }
        public bool Lvl1Dependency { get; set; }
        public bool Lvl2Dependency { get; set; }
        public List<DecorationAvailableDTO> NextLvlDecorations { get; set; }

        public DecorationAvailableDTO(String TypeName, bool Lvl1Dependency, bool Lvl2Dependency)
        {
            this.TypeName = TypeName;
            this.Lvl1Dependency = Lvl1Dependency;
            this.Lvl2Dependency = Lvl2Dependency;
            NextLvlDecorations = new List<DecorationAvailableDTO>();
        }
    }
}
