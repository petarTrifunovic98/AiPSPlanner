using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class VoteProfiles : Profile
    {
        public VoteProfiles()
        {

            CreateMap<Votable, VotableDTO>()
                .ReverseMap();

            CreateMap<Vote, VoteDTO>()
                .ForMember(dest =>
                    dest.User,
                    opt => opt.MapFrom(
                        src => src.User))
                .ForMember(dest =>
                    dest.Votable,
                    opt => opt.MapFrom(
                        src => src.Votable))
                .ReverseMap();

            CreateMap<VoteCreateDTO, Vote>();
            CreateMap<Vote, VoteEditDTO>();
        }
    }
}
