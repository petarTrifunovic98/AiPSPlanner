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
        public ItemProfiles()
        {

            CreateMap<Item, ItemDTO>()
                .ForMember(dest =>
                    dest.User,
                    opt => opt.MapFrom(
                        src => src.User))
                .ForMember(dest =>
                    dest.Trip,
                    opt => opt.MapFrom(
                        src => src.Trip))
                .ReverseMap();
        }
    }
}
