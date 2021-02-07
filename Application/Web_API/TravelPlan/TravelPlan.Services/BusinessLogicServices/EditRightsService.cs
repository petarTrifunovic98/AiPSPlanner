using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.Services.MessagingService;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class EditRightsService: IEditRightsService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private MessageControllerService _messageControllerService;

        public EditRightsService(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IHubContext<MessageHub> hubContext)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
            _messageControllerService = new MessageControllerService(hubContext);
        }

        public async Task<bool> HasEditRights(int tripId)
        {
            User user = (User)_contextAccessor.HttpContext.Items["User"];
            string tokenId = user.UserId.ToString();
            using (_unitOfWork)
            {
                string currentRightHolder = await _unitOfWork.EditRightsRepository.GetCurrentRightHolder(tripId);
                return tokenId == currentRightHolder;
            }
        }

        public async Task<bool> RequestTripEdit(int tripId, int userId)
        {
            using (_unitOfWork)
            {
                long requestsInList = await _unitOfWork.EditRightsRepository.RequestTripEdit(tripId, userId);
                if (requestsInList == 1)
                {
                    await _unitOfWork.EditRightsRepository.SetEditRightHolder(tripId, userId);
                    return true;
                }
                return false;
            }
        }

        public async Task ReleaseTripEdit(int tripId)
        {
            using (_unitOfWork)
            {
                string nextUserIdString = await _unitOfWork.EditRightsRepository.GetNextRightHolder(tripId);
                if (!string.IsNullOrEmpty(nextUserIdString))
                {
                    await _unitOfWork.EditRightsRepository.SetEditRightHolder(tripId, int.Parse(nextUserIdString));
                    await _messageControllerService.SendNotification(int.Parse(nextUserIdString), "EditRightsNotification", tripId);
                }
            }
        }

        public async Task LeaveRequestEditQueue(int tripId, int userId)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.EditRightsRepository.RemoveUserFromRequestQueue(tripId, userId);
            }
        }
    }
}
