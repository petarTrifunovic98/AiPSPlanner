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
                        src => _mapper.Map<Accommodation, AccommodationDTO>(src.Accommodation)));

            CreateMap<Accommodation, AccommodationDTO>()
                .ForMember(dest =>
                    dest.Pictires,
                    opt => opt.MapFrom(
                        src => _mapper.Map<ICollection<AccommodationPicture>, ICollection<AccommodationDTO>>(src.Pictires)));
        }
    }
}
