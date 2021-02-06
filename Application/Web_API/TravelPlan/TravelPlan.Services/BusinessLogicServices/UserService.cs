using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Services.AuthentificationService;
using TravelPlan.Helpers;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TravelPlan.Services.MessagingService;
using StackExchange.Redis;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ITokenManager _tokenManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHubContext<MessageHub, IMessageHub> _messageHub;
        private readonly IEditRightsAuthorization _editRightsAuthorization;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ITokenManager tokenManager, 
            IHttpContextAccessor contextAccessor, IHubContext<MessageHub, IMessageHub> messageHub, IEditRightsAuthorization editRightsAuthorization)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _tokenManager = tokenManager;
            _contextAccessor = contextAccessor;
            _messageHub = messageHub;
            _editRightsAuthorization = editRightsAuthorization;
        }

        public async Task<UserAuthenticateResponseDTO> AddUserAccount(UserRegisterDTO userInfo)
        {
            using (_unitOfWork)
            {
                if (_unitOfWork.UserRepository.UsernameTaken(userInfo.Username))
                    return null;

                userInfo.Password = PasswordEncryptionService.EncryptPassword(userInfo.Password, _appSettings.SaltLength);
                User newUser = _mapper.Map<UserRegisterDTO, User>(userInfo);
                await _unitOfWork.UserRepository.Create(newUser);
                await _unitOfWork.Save();
                UserAuthenticateResponseDTO returnUser = _mapper.Map<User, UserAuthenticateResponseDTO>(newUser);
                returnUser.Token = _tokenManager.GenerateToken(newUser.UserId);
                returnUser.UnseenNotifications = _unitOfWork.UserRepository.GetUnseenNotificationNumber(newUser.UserId);
                return returnUser;
            }
        }

        public async Task<UserAuthenticateResponseDTO> LogUserIn(UserLoginDTO userInfo)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.GetUserByUsername(userInfo.Username);
                if (user == null)
                    return null;
                if (!PasswordEncryptionService.IsPasswordCorrect(user.Password, userInfo.Password, _appSettings.SaltLength))
                    return null;

                UserAuthenticateResponseDTO returnUser = _mapper.Map<User, UserAuthenticateResponseDTO>(user);
                returnUser.Token = _tokenManager.GenerateToken(user.UserId);
                returnUser.UnseenNotifications = _unitOfWork.UserRepository.GetUnseenNotificationNumber(user.UserId);
                return returnUser;
            }
        }

        public async Task<bool> LogUserOut(int userId)
        {
            User user = (User)_contextAccessor.HttpContext.Items["User"];
            int tokenId = user.UserId;

            if (tokenId == userId)
            { 
                await _tokenManager.DeactivateCurrentAsync();
                return true;
            }
            return false;
        }

        public async Task<UserDTO> EditUserInfo(UserEditDTO userInfo)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userInfo.UserId);
                user.Name = userInfo.Name;
                user.LastName = userInfo.LastName;
                if (!string.IsNullOrEmpty(userInfo.Picture))
                    user.Picture = PictureManagerService.SaveImageToFile(userInfo.Picture, user.GetType().Name, user.UserId);

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.Save();

                if (userInfo.Picture != null)
                    user.Picture = null;

                UserDTO returnUser = _mapper.Map<User, UserDTO>(user);

                if (userInfo.Picture != null)
                    returnUser.Picture = userInfo.Picture;

                return returnUser;
            }
        }

        public async Task<bool> ChangePassword(UserChangePassDTO userInfo)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userInfo.UserId);
                if (!PasswordEncryptionService.IsPasswordCorrect(user.Password, userInfo.OldPassword, _appSettings.SaltLength))
                    return false;

                user.Password = PasswordEncryptionService.EncryptPassword(userInfo.NewPassword, _appSettings.SaltLength);
                _unitOfWork.UserRepository.Update(user);

                return await _unitOfWork.Save();
            }
        }

        public async Task<bool> DeletePicture(int userID)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userID);
                user.Picture = null;
                await _unitOfWork.Save();
                return true;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            using(_unitOfWork)
            {
                IEnumerable<User> users = await _unitOfWork.UserRepository.FindAll();
                IEnumerable<UserDTO> usersInfos = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
                return usersInfos;
            }
        }

        public async Task<UserDTO> GetSpecificUser(int userId)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                return _mapper.Map<User, UserDTO>(user);
            }
        }

        public async Task<UserBasicDTO> GetUserByUsername(String username)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.GetUserByUsername(username);
                return _mapper.Map<User, UserBasicDTO>(user);
            }
        }

        public async Task<bool> RequestTripEdit(int tripId, int userId)
        {
            using (_unitOfWork)
            {
                long requestsInList = await _unitOfWork.UserRepository.RequestTripEdit(tripId, userId);
                if(requestsInList == 1)
                {
                    await _unitOfWork.UserRepository.SetEditRightHolder(tripId, userId);
                    return true;
                }
                return false;
            }
        }

        public async Task ReleaseTripEdit(int tripId)
        {
            using (_unitOfWork)
            {
                string currentId = await _unitOfWork.UserRepository.GetCurrentRightHolder(tripId);
                bool hasRights = _editRightsAuthorization.HasEditRights(currentId);

                string nextUserIdString = await _unitOfWork.UserRepository.GetNextRightHolder(tripId);
                if(!string.IsNullOrEmpty(nextUserIdString))
                {
                    await _unitOfWork.UserRepository.SetEditRightHolder(tripId, int.Parse(nextUserIdString));
                    //inform the next user through his client that he/she now holds the edit rights for this trip
                }
            }
        }

        public async Task LeaveRequestEditQueue(int tripId, int userId)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.UserRepository.RemoveUserFromRequestQueue(tripId, userId);
            }
        }
    }
}
