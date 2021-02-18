using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Helpers;

namespace TravelPlan.DataAccess.Entities
{
    public class SeaType : TripType
    {
        private static string _seaIcon;
        private static string _seaIconPath = "TripTypeIcons\\sea.jpg";
        private static object _lock = new object();

        public SeaType()
        {
            this.StandardList = "Swim suit_Flip-flops_Hat_Water shoes_Sunglasses_Beach bag_Sunscreen_Moisturizer_After sun cream_"
                                + "Beach umbrella_Snorkel gear_Portable speaker_Beach towel_Beach sports and water toys_Flotation devices_"
                                + "Cooler_Waterproof phone case";
        }

        public override List<string> getTipsAndTricks()
        {
            List<String> retList = new List<String>();
            retList.Add("1. Choose the Right Beach for the Type of Vacation You Want.\n" +
                        "Do Your Research\n" +
                        "Start by deciding what type of experience you want from your stay.\n" +
                        "Do you enjoy lively crowds or would you rather read a book and to listen to the lapping ocean waves in relative quiet ? Are you traveling with young children or would you prefer mostly adult company ? Do you want to bring your dog along or would you rather be somewhere that doesn't allow pets? Do you want to avoid biting insects? Do you plan to spend most of your time on the sand or in the water? Do your research first to find a beach that has the type of environment you'll enjoy most.");
            retList.Add("2. Bring a Comfortable Place to Sit or Sunbathe.\n" +
                        "An oversized, plush and thirsty beach towel and / or a comfortable portable chair or folding recliner will give you a comfortable base for your fun in the sun.\n" +
                        "Do you want something close to the ground or would you be more comfortable in something with a higher seat ? Do you want something with a drink holder ? Something that's lightweight or has a strap that makes it easy to carry? If you're not going alone, what would the others traveling with you prefer ? Will you be bringing a toddler or young child along ? What type of seating would make them most comfortable ?\n" +
                        "Are the beach chairs or portable loungers you have in good shape, or have they started to rust ? Are they comfortable ?\n" +
                        "Are your towels large enough to keep you sand - free when you lie down on them after swimming in the ocean ? Are they colorful and distinctive enough so it's easy for you - and others - to identify which towel is yours? A little pre-trip planning will ensure that you'll be able to sit, recline, or lie back in comfort while you enjoy the sand and surf.");
            retList.Add("3. Protect Yourself.\n" +
                        "Nothing can ruin a beach vacation faster than a sunburn, itchy bug bites, or a cut on the foot! So be prepared to deal with them so they won't spoil your trip.\n" +
                        "Protect Your Skin From the Sun’s Damaging UVA / UVB Rays\n" +
                        "Buy a new bottle of sunscreen to pack to make sure you won’t run out, and be sure check the expiration date. The Skin Cancer Foundation recommends using a broad spectrum sunscreen formula for UVA and UVB protection with an SPF of 15 or higher. For extended outdoor activity, they recommend a water-resistant, broad spectrum (UVA/UVB) sunscreen with an SPF of 30 or higher.\n" +
                        "Consider using two different sunscreens, one for your face and another for your body – especially if your skin is sensitive or prone to acne breakouts. More and more brands of sunscreen are offering a variety of sensitive skin formulations for both face and body.");
            retList.Add("4. Be Prepared for Mishaps and Emergencies.\n" +
                        "Remember Ben Franklin's famous saying, 'An ounce of prevention is worth a pound of cure.' You've armed yourself with an effective, high SPF sunblock and are applying and reapplying it liberally throughout the day. You've checked about the biting insect situation, brought an appropriate insect repellent, if appropriate, and are following the package directions carefully. You're protecting your eyes with sunglasses that block 99 to 100 percent of both UVA and UVB rays whenever your outdoors. You've bought and are wearing appropriate sandals and/or water shoes to protect your feet. And you've stashed any keys and valuables you want to take with you to the beach inside a waterproof, airtight, crush-proof dry box. You're definitely a smart cookie! It's evern smarter to expect the unexpected and be as prepared as possible to handle any mishaps.");
            return retList;
        }

        public override string getIcon()
        {
            if (_seaIcon == null)
            {
                lock (_lock)
                {
                    if (_seaIcon == null)
                        _seaIcon = PictureManagerService.LoadImageFromFile(_seaIconPath);
                }
            }
            return _seaIcon;
        }
    }
}
