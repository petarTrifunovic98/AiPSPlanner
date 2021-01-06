using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class UserProfiles : Profile
    {
        private readonly IMapper _mapper;
        public UserProfiles(IMapper mapper)
        {
            _mapper = mapper;

            CreateMap<User, UserDTO>()
                .ForMember(dest =>
                    dest.MyTeams,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<Team>, ICollection<TeamDTO>>(src.MyTeams)))
                .ForMember(dest =>
                    dest.MyItems,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<Item>, ICollection<ItemDTO>>(src.MyItems)))
                .ForMember(dest =>
                    dest.MyTrips,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<Trip>, ICollection<TripDTO>>(src.MyTrips)))
                .ReverseMap();
        }
    }
}
