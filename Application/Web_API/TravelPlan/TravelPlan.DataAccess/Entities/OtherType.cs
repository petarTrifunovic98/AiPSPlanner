using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Helpers;

namespace TravelPlan.DataAccess.Entities
{
    public class OtherType : TripType
    {
        private static string _otherIcon;
        private static string _otherIconPath = "TripTypeIcons\\other.webp";
        private static object _lock = new object();

        public OtherType()
        {
            this.StandardList = "";
        }

        public override string getIcon()
        {
            if (_otherIcon == null)
            {
                lock (_lock)
                {
                    if (_otherIcon == null)
                        _otherIcon = PictureManagerService.LoadImageFromFile(_otherIconPath);
                }
            }
            return _otherIcon;
        }

        public override List<string> getTipsAndTricks()
        {
            return new List<String>();
        }
    }
}
