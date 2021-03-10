using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    class AddOnProfiles : Profile
    {
        public AddOnProfiles()
        {
            CreateMap<AddOn, AddOnDTO>()
                .ForMember(dest =>
                    dest.Votable,
                    opt => opt.MapFrom(
                        src => src.Votable))
                .ForMember(dest =>
                    dest.Lvl1DependId,
                    opt => opt.MapFrom(
                        src => src.GetLvl1DependId()))
                .ForMember(dest =>
                    dest.Lvl2DependId,
                    opt => opt.MapFrom(
                        src => src.GetLvl2DependId()))
                .ForMember(dest =>
                    dest.Type,
                    opt => opt.MapFrom(
                        src => src.GetType().Name))
                .ReverseMap();

            CreateMap<AddOnCreateDTO, Cruise>();
            CreateMap<AddOnCreateDTO, Lunch>();
            CreateMap<AddOnCreateDTO, Breakfast>();
            CreateMap<AddOnCreateDTO, Tea>();
            CreateMap<AddOnCreateDTO, Coffee>();
            CreateMap<AddOnCreateDTO, Juice>();
            CreateMap<AddOnCreateDTO, Wine>();
            CreateMap<AddOnCreateDTO, Dessert>();
            CreateMap<AddOnCreateDTO, Aquapark>();
            CreateMap<AddOnCreateDTO, Waterboard>();
            CreateMap<AddOnCreateDTO, SunBeds>();
            CreateMap<AddOnCreateDTO, SkiPass>();
            CreateMap<AddOnCreateDTO, SkiEquipment>();
            CreateMap<AddOnCreateDTO, Snowboard>();
            CreateMap<AddOnCreateDTO, Skis>();
            CreateMap<AddOnCreateDTO, SkiPoles>();
            CreateMap<AddOnCreateDTO, SkiBoots>();
            CreateMap<AddOnCreateDTO, Sled>();
            CreateMap<AddOnCreateDTO, BikeRent>();
            CreateMap<AddOnCreateDTO, ScooterRent>();
            CreateMap<AddOnCreateDTO, Walk>();
            CreateMap<AddOnCreateDTO, TourGuide>();
            CreateMap<AddOnCreateDTO, TrainTour>();
            CreateMap<AddOnCreateDTO, Meal>();
            CreateMap<AddOnCreateDTO, Pogaca>();
            CreateMap<AddOnCreateDTO, Schnapps>();
        }
    }
}
