using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class TeamProfile : Profile
    {
        private readonly IMapper _mapper;
        public TeamProfile(IMapper mapper)
        {
            _mapper = mapper;

            CreateMap<Team, TeamDTO>()
                .ForMember(dest =>
                    dest.Members,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<User>, ICollection<UserDTO>>(src.Members)))
                .ReverseMap();
        }
    }
}
