using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.DTOs.Profiles
{
    public class AccommodationProfiles : Profile
    {

        public AccommodationProfiles()
        {

            CreateMap<AccommodationPicture, AccommodationPictureDTO>()
                .ForMember(dest =>
                    dest.Accommodation,
                    opt => opt.MapFrom(
                        src => src.Accommodation))
                .ReverseMap();

            CreateMap<Accommodation, AccommodationDTO>()
                .ForMember(dest =>
                    dest.Pictures,
                    opt => opt.MapFrom(
                        src => src.Pictures))
                .ForMember(dest =>
                    dest.Location,
                    opt => opt.MapFrom(
                        src => src.Location))
                .ForMember(dest =>
                    dest.AccommodationId,
                    opt => opt.MapFrom(
                        src => src.VotableId))
                .ReverseMap();

            CreateMap<AccommodationCreateDTO, Accommodation>();
            CreateMap<AccommodationEditDTO, Accommodation>()
                .ForMember(dest =>
                    dest.VotableId,
                    opt => opt.MapFrom(
                        src => src.AccommodationId));
        }
    }
}
