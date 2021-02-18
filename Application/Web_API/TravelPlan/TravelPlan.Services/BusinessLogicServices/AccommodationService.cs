using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Helpers;
using TravelPlan.Services.MessagingService;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private MessageControllerService _messageControllerService;

        public AccommodationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<MessageHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageControllerService = new MessageControllerService(hubContext);
        }

        public async Task<AccommodationDTO> CreateAccommodation(AccommodationCreateDTO newAccommodation)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(newAccommodation.From, newAccommodation.To))
                    return null;

                Accommodation accommodation = _mapper.Map<AccommodationCreateDTO, Accommodation>(newAccommodation);
                Location location = await _unitOfWork.LocationRepository.FindByID(newAccommodation.LocationId);

                if (!DateManagerService.checkParentChildDates(location.From, location.To, accommodation.From, accommodation.To))
                    return null;

                accommodation.Location = location;

                if (location.Accommodations == null)
                    location.Accommodations = new List<Accommodation>();
                location.Accommodations.Add(accommodation);

                Votable votable = new Votable();
                accommodation.Votable = votable;

                await _unitOfWork.VotableRepository.Create(votable);
                await _unitOfWork.AccommodationRepository.Create(accommodation);
                await _unitOfWork.Save();

                AccommodationDTO retValue = _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
                await _messageControllerService.NotifyOnTripChanges(location.TripId, "AddAccommodation", retValue);
                return retValue;
            }
        }

        public async Task<bool> DeleteAccommodation(int accommodationId)
        {
            using (_unitOfWork)
            {
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.GetAccommodationWithLocation(accommodationId);
                _unitOfWork.VotableRepository.Delete(accommodation.VotableId);
                _unitOfWork.AccommodationRepository.Delete(accommodationId);
                await _unitOfWork.Save();

                AccommodationRemoveDTO removeAcc = _mapper.Map<Accommodation, AccommodationRemoveDTO>(accommodation);
                await _messageControllerService.NotifyOnTripChanges(accommodation.Location.TripId, "RemoveAccommodation", removeAcc);
                return true;
            }
        }

        public async Task<AccommodationDTO> EditAccommodationInfo(AccommodationEditDTO accommodationInfo)
        {
            using (_unitOfWork)
            {
                if (!DateManagerService.checkFromToDates(accommodationInfo.From, accommodationInfo.To))
                    return null;

                Accommodation accommodation = await _unitOfWork.AccommodationRepository.FindByID(accommodationInfo.AccommodationId);
                Location location = await _unitOfWork.LocationRepository.FindByID(accommodation.LocationId);

                accommodation.Type = accommodationInfo.Type;
                accommodation.Name = accommodationInfo.Name;
                accommodation.Description = accommodationInfo.Description;
                accommodation.From = accommodationInfo.From;
                accommodation.To = accommodationInfo.To;
                accommodation.Address = accommodationInfo.Address;

                if (!DateManagerService.checkParentChildDates(location.From, location.To, accommodation.From, accommodation.To))
                    return null;

                await _unitOfWork.Save();

                AccommodationDTO retValue = _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
                await _messageControllerService.NotifyOnTripChanges(location.TripId, "EditAccommodation", retValue);
                return retValue;
            }
        }

        public IEnumerable<string> GetAccommodationTypes()
        {
            return Enum.GetNames(typeof(AccommodationType));
        }

        public async Task<AccommodationDTO> GetSpecificAccommodation(int accommodationId)
        {
            using(_unitOfWork)
            {
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.GetAccommodationWithVotable(accommodationId);
                return _mapper.Map<Accommodation, AccommodationDTO>(accommodation);
            }
        }

        public async Task<List<AccommodationDTO>> GetAccommodationsForLocation(int locationId)
        {
            using (_unitOfWork)
            {
                Location location = await _unitOfWork.LocationRepository.GetLocationWithAccommodations(locationId);
                return location.Accommodations.Select(a => _mapper.Map<Accommodation, AccommodationDTO>(a)).ToList();
            }
        }

        public async Task<AccommodationPictureDTO> AddAccommodationPicture(AccommodationPictureCreateDTO picture)
        {
            using (_unitOfWork)
            {
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.GetAccommodationWithLocation(picture.AccommodationId);
                AccommodationPicture accommodationPicture = new AccommodationPicture
                {
                    AccommodationPictureId = 0,
                    AccommodationId = accommodation.AccommodationId,
                    Accommodation = accommodation,
                    Picture = "temp",
                };

                await _unitOfWork.AccommodationPictureRepository.Create(accommodationPicture);
                await _unitOfWork.Save();

                accommodationPicture.Picture = PictureManagerService.SaveImageToFile(picture.Picture, accommodation.GetType().Name, accommodationPicture.AccommodationPictureId);
                accommodationPicture.Accommodation = accommodation;

                if (accommodation.Pictures == null)
                    accommodation.Pictures = new List<AccommodationPicture>();
                accommodation.Pictures.Add(accommodationPicture);
                _unitOfWork.AccommodationPictureRepository.Update(accommodationPicture);
                await _unitOfWork.Save();
                accommodationPicture.Picture = null;

                AccommodationPictureDTO res = _mapper.Map<AccommodationPicture, AccommodationPictureDTO>(accommodationPicture);
                res.Picture = picture.Picture;
                await _messageControllerService.NotifyOnTripChanges(accommodation.Location.TripId, "AddAccommodationPicture", res);
                return res;
            }
        }

        public async Task<IEnumerable<AccommodationPictureDTO>> GetAccommodationPictures(int accommodationId)
        {
            using (_unitOfWork)
            {
                IEnumerable<AccommodationPicture> pictures = await _unitOfWork.AccommodationRepository.GetAccommodationPictures(accommodationId);
                return _mapper.Map<IEnumerable<AccommodationPicture>, IEnumerable<AccommodationPictureDTO>>(pictures);
            }
        }

        public async Task DeleteAccommodationPicture(int pictureId)
        {
            using (_unitOfWork)
            {
                AccommodationPicture accommodationPicture = await _unitOfWork.AccommodationPictureRepository.FindByID(pictureId);
                Accommodation accommodation = await _unitOfWork.AccommodationRepository.GetAccommodationWithLocation(accommodationPicture.AccommodationId);
                AccommodationPictureDTO deleteInfo = _mapper.Map<AccommodationPicture, AccommodationPictureDTO>(accommodationPicture);
                deleteInfo.Picture = "";
                _unitOfWork.AccommodationPictureRepository.Delete(pictureId);
                await _unitOfWork.Save();
                await _messageControllerService.NotifyOnTripChanges(accommodation.Location.TripId, "RemoveAccommodationPicture", deleteInfo);
            }
        }
    }
}
