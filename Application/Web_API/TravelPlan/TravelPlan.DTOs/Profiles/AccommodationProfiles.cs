using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Helpers;

namespace TravelPlan.DTOs.Profiles
{
    public class AccommodationProfiles : Profile
    {

        public AccommodationProfiles()
        {

            CreateMap<AccommodationPicture, AccommodationPictureDTO>()
                .ForMember(dest =>
                    dest.LocationId,
                    opt => opt.MapFrom(
                        src => src.Accommodation.LocationId))
                .ForMember(dest => 
                    dest.Picture,
                    opt=>opt.MapFrom(
                        src=>PictureManagerService.LoadImageFromFile(src.Picture)));

            CreateMap<AccommodationPictureCreateDTO, AccommodationPicture>();

            CreateMap<Accommodation, AccommodationDTO>()
                .ForMember(dest =>
                    dest.Pictures,
                    opt => opt.MapFrom(
                        src => src.Pictures))
                .ForMember(dest =>
                    dest.Votable,
                    opt => opt.MapFrom(
                        src => src.Votable))
                .ForMember(dest =>
                    dest.Type,
                    opt => opt.MapFrom(
                        src => src.Type.ToString()))
                .ReverseMap();

            CreateMap<AccommodationCreateDTO, Accommodation>();
            CreateMap<AccommodationEditDTO, Accommodation>();
            CreateMap<Accommodation, AccommodationRemoveDTO>();
        }
    }
}
