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
        private readonly IMapper _mapper;
        public AccommodationProfiles(IMapper mapper)
        {
            _mapper = mapper;

            CreateMap<AccommodationPicture, AccommodationPictureDTO>()
                .ForMember(dest =>
                    dest.Accommodation,
                    opt => opt.MapFrom(
                        src => _mapper.Map<Accommodation, AccommodationDTO>(src.Accommodation)))
                .ReverseMap();

            CreateMap<Accommodation, AccommodationDTO>()
                .ForMember(dest =>
                    dest.Pictures,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<AccommodationPicture>, ICollection<AccommodationPictureDTO>>(src.Pictures)))
                .ForMember(dest =>
                    dest.Location,
                    opt => opt.MapFrom(
                        src => _mapper.Map<Location, LocationDTO>(src.Location)))
                .ReverseMap();
        }
    }
}
