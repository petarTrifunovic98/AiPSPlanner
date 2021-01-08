using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class TripProfiles : Profile
    {
        public TripProfiles()
        {
            CreateMap<Trip, TripDTO>()
                .ForMember(dest =>
                    dest.Locations,
                    opt => opt.MapFrom(
                        src => src.Locations))
                .ForMember(dest =>
                    dest.Travelers,
                    opt => opt.MapFrom(
                        src => src.Travelers))
                .ForMember(dest =>
                    dest.ItemList,
                    opt => opt.MapFrom(
                        src => src.ItemList))
                .ReverseMap();

            CreateMap<TripCreateDTO, Trip>();
        }
    }
}
