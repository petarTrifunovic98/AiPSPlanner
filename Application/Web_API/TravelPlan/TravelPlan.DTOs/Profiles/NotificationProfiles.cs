using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class NotificationProfiles : Profile
    {
        public NotificationProfiles()
        {
            CreateMap<NotificationDTO, Notification>().ReverseMap();
        }
    }
}
