using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class ItemProfiles : Profile
    {
        private readonly IMapper _mapper;
        public ItemProfiles(IMapper mapper)
        {
            _mapper = mapper;

            CreateMap<Item, ItemDTO>()
                .ForMember(dest =>
                    dest.User,
                    opt => opt.MapFrom(
                        src => _mapper.Map<User, UserDTO>(src.User)))
                .ForMember(dest =>
                    dest.Trip,
                    opt => opt.MapFrom(
                        src => _mapper.Map<Trip, TripDTO>(src.Trip)))
                .ReverseMap();
        }
    }
}
