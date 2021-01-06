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
        private readonly IMapper _mapper;
        public TripProfiles(IMapper mapper)
        {
            _mapper = mapper;

            CreateMap<Trip, TripDTO>()
                .ForMember(dest =>
                    dest.Locations,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<Location>, ICollection<LocationDTO>>(src.Locations)))
                .ForMember(dest =>
                    dest.Travelers,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<User>, ICollection<UserDTO>>(src.Travelers)))
                .ForMember(dest =>
                    dest.ItemList,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<Item>, ICollection<ItemDTO>>(src.ItemList)))
                .ReverseMap();
        }
    }
}
