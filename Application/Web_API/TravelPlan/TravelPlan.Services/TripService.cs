using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;

namespace TravelPlan.Services
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TripService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
