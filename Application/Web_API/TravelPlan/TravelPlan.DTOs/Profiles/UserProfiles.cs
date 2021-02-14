using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Helpers;

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
                .ForMember(dest =>
                    dest.Picture,
                    opt => opt.MapFrom(
                        src => PictureManagerService.LoadImageFromFile(src.Picture)));

            CreateMap<User, UserBasicDTO>();

            CreateMap<UserRegisterDTO, User>();

            CreateMap<UserEditDTO, User>();

            CreateMap<User, UserAuthenticateResponseDTO>()
                .ForMember(dest =>
                    dest.Picture,
                    opt => opt.MapFrom(
                        src => PictureManagerService.LoadImageFromFile(src.Picture)));
        }
    }
}
