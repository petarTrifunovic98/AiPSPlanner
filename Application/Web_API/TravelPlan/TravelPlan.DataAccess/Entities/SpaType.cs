using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Helpers;

namespace TravelPlan.DataAccess.Entities
{
    public class SpaType : TripType
    {
        private static string _spaIcon;
        private static string _spaIconPath = "TripTypeIcons\\spa.png";
        private static object _lock = new object();

        public SpaType()
        {
            this.StandardList = "Book_Comfortable shoes_Hiking shoes_Portable speaker_Backpack_Large water bottle_Sports wear_"
                                + "Warm clothing_Layered clothing_Wind jacket_Hat_Sunscreen_Sunglassess_Sports gear_Backpack_"
                                + "First aid kit_Lip balm_Rain coat_Board games_Tea";
        }

        public override List<string> getTipsAndTricks()
        {
            List<String> retList = new List<String>();
            retList.Add("Nature is Beautiful\n" +
                        "While larger cities may boast impressive skylines, there is nothing quite as beautiful as nature. It is hard not to be awestruck in the presence of an alpine meadow in full bloom, a golden seaside landscape at sunrise, or a sprawling red rock canyon.\n" +
                        "Perhaps the captivating presence of nature stems from the fact that it is alive, powerful and ever changing. A glacier is spectacular because of the way it carves through stone and shapes valleys. Even when all seems dead in the thick of winter, renewal is taking place below the surface.");
            retList.Add("Physical Activity is Built-In\n" +
                        "Most nature vacations involve some level of fitness. If you're on a multi-day trek, your vacation is also your exercise. A holiday spent at the cabin could involve kayaking on a nearby freshwater lake or putting in some miles on your mountain bike.\n" +
                        "One exception to this trend could be a beach holiday, but even then, it's easy to incorporate some activity by opting to snorkel, scuba dive, surf, or paddleboard.\n" +
                        "While including physical activity in your vacation can be difficult at times, what better way to stay in shape than while exploring the world and enjoying nature?");
            retList.Add("Improved Mood and Health\n" +
                        "The benefits of nature travel don't stop at improving your fitness. Research has shown that being in the outdoors, even for just a short period, can improve your mood and reduce stress.\n" +
                        "Immersion in nature can evoke pleasant feelings and reduce a negative mental state along with emotions of anger, hopelessness, anxiety and fear. In turn, positive changes to your mood can improve your physical health, specifically your endocrine, parasympathetic nervous and immune systems. If you are active during your nature vacation, you likely will have an endorphin rush which helps to relieve stress and anxiety even further.\n" +
                        "Some of these benefits can be reaped after only half an hour in nature, so imagine how much better you will feel if your whole getaway is spent amongst the outdoors.");
            return retList;
        }

        public override string getIcon()
        {
            if (_spaIcon == null)
            { 
                lock(_lock)
                { 
                    if(_spaIcon == null)
                        _spaIcon = PictureManagerService.LoadImageFromFile(_spaIconPath);
                }
            }
            return _spaIcon;
        }
    }
}
