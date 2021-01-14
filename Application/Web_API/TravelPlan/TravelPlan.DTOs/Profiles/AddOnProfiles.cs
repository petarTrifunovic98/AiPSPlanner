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
        }
    }
}
