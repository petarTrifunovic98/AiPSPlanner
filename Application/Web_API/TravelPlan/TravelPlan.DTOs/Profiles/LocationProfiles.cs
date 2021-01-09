using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class LocationProfiles : Profile
    {
        public LocationProfiles()
        {

            CreateMap<Location, LocationDTO>()
                .ForMember(dest =>
                    dest.Accommodations,
                    opt => opt.MapFrom(
                        src => src.Accommodations))
                .ForMember(dest =>
                    dest.Trip,
                    opt => opt.MapFrom(
                        src => src.Trip))
                .ReverseMap();

            CreateMap<LocationCreateDTO, Location>();
            CreateMap<LocationEditDTO, Location>();
        }
    }
}
