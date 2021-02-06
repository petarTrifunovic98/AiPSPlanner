using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class EditRightsService: IEditRightsService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public EditRightsService(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
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
                    //inform the next user through his client that he/she now holds the edit rights for this trip
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
