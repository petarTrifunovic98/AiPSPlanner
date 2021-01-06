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
        private readonly IMapper _mapper;
        public LocationProfiles(IMapper mapper)
        {
            _mapper = mapper;

            CreateMap<Location, LocationDTO>()
                .ForMember(dest =>
                    dest.Accommodations,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<Accommodation>, ICollection<AccommodationDTO>>(src.Accommodations)))
                .ForMember(dest =>
                    dest.Trip,
                    opt => opt.MapFrom(
                        src => _mapper.Map<Trip, TripDTO>(src.Trip)))
                .ReverseMap();
        }
    }
}
