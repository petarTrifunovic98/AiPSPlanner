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
        public UserProfiles()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest =>
                    dest.MyTeams,
                    opt => opt.MapFrom(
                        src => src.MyTeams))
                .ForMember(dest =>
                    dest.MyItems,
                    opt => opt.MapFrom(
                        src => src.MyItems))
                .ForMember(dest =>
                    dest.MyTrips,
                    opt => opt.MapFrom(
                        src => src.MyTrips))
                .ReverseMap();

            CreateMap<UserRegisterDTO, User>();

            CreateMap<UserEditDTO, User>();
        }
    }
}
