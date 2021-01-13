using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Helpers;

namespace TravelPlan.DataAccess.Entities
{
    public class WinterType : TripType
    {
        private static string _winterIcon;
        private static string _winterIconPath = "TripTypeIcons\\winter.png";
        private static object _lock = new object();

        public WinterType()
        {
            this.StandardList = "Waterproof ski jacket_Waterproof ski pants_Skis/Snowboard_Ski poles_Ski boots_Helmet_Goggle_"
                                + "Waterproof gloves_Ski socks_Balaclava_Base layer clothing_Skull cap_Fleece jacke_Puffy jacket_Hoodies_"
                                + "Pocket sized sunscreen_First aid kit_Lip balm_Goggles wiper_Pocket tissues_Small backpack_Water bottle_"
                                + "Waterproof boots_Muscle relief cream_Sunglasses_Ski tickets";
        }

        public override List<string> getTipsAndTricks()
        {
            List<String> retList = new List<String>();
            retList.Add("WHICH SKI RESORT?\n" +
                        "These days, most major ski resorts offer a laundry list of on - and off - mountain experiences, services and amenities.You can expect high - touch services, like free on - mountain tours; state - of - the - art facilities, like spacious lodges, ski school centers and cutting - edge lifts, gondolas and trams; and awesome activities, like groomed, lift-accessed snow tubing, snowmobile excursions and festive outdoor ice rinks. Additionally, world -class resorts offer a stacked-list of events, parties, festivals and concerts, which provide the perfect cherry on top of an awesome ski vacation.All of this and more is commonplace at modern ski resorts.\n" +
                        "Choosing the right resort for you comes down to a couple things: your budget, the type of terrain you want to ski or ride, the atmosphere you want to be surrounded by and how easy it is to get there.Some people prefer end-of-the-road resorts, while others crave the convenience and high-energy buzz of well-visited destinations. The choice is yours, but we're here to help you make it.");
            retList.Add("WHAT KIND OF LODGING?\n" +
                        "Selecting your lodging will depend a lot on your budget, but the size of your travel party can also influence your choice. If being close to the slopes is important to you, ski in ski out lodging is recommended. It's a great option for first-time visitors, too, as it reduces the need to navigate a base village or town. Plus, you likely won't need a rental car or to schlep your gear on a shuttle if you're staying slopeside. However, ski in ski out lodging typically doesn't come on the cheap. You're paying extra for the center-stage seats.\n" +
                        "If you're family or group is large, renting a private home or condo unit is also a great option. Everyone can congregate in the living rooms and common-area spaces and enjoy meals together in the fully equipped kitchens. Plus, when split between several people, a private home or condo can be more affordable than a hotel room or suite.");
            retList.Add("DO I NEED A LESSON?\n" +
                        "A lesson is highly reccommended for first-time skiers or snowboarders. Not only will you receive instruction on how to properly stop and turn, but you'll also get a tutorial on how to operate your equipment and load and unload lifts." +
                        "We also recommend a lesson for first-time visitors, even if you're expert. A lesson is the best way to get your legs warmed and fine-tune your technique and style. Plus, instructors provide a wealth of insight on terrain and off-mountain experiences that you can't get anywhere else. You'll also get to cut long lift lines when you're skiing or riding with an instructor.");
            return retList;
        }

        public override string getIcon()
        {
            if (_winterIcon == null)
            {
                lock (_lock)
                {
                    if (_winterIcon == null)
                        _winterIcon = PictureManagerService.LoadImageFromFile(_winterIconPath);
                }
            }
            return _winterIcon;
        }
    }
}
