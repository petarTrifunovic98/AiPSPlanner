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
        public TeamProfile()
        {
            CreateMap<Team, TeamDTO>()
                .ForMember(dest =>
                    dest.Members,
                    opt => opt.MapFrom(
                        src => src.Members))
                .ReverseMap();

            CreateMap<CreateTeamDTO, Team>();
        }
    }
}
