using System;
using System.Collections.Generic;
using System.Text;

namespace TravelPlan.DTOs.DTOs
{
    public enum AddOnType
    {
        Sunbeds,
        Waterboard,
        Aquapark,
        Cruise,
        Lunch,
        Breakfast,
        Coffee,
        Tea,
        Juice,
        Wine,
        Dessert,
        SkiPass,
        SkiEquipment,
        Snowboard,
        Skis,
        SkiPoles,
        SkiBoots,
        Sled,
        BikeRent,
        ScooterRent,
        Walk,
        TourGuide,
        TrainTour,
        Meal,
        Pogaca,
        Schnapps
    }

    public class AddOnCreateDTO
    {
        public int TripId { get; set; }
        public AddOnType AddOnType { get; set; }
        public int Price { get; set; }
        public String Description { get; set; }

        public int Lvl1DependId { get; set; }
        public int Lvl2DependId { get; set; }
    }
}
