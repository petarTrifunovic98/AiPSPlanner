using AutoMapper;
using System;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDTO> AddUserAccount(UserRegisterDTO userInfo)
        {
            using(_unitOfWork)
            {
                if (_unitOfWork.UserRepository.UsernameTaken(userInfo.Username))
                    return null;

                byte[] pwdBytes = System.Text.Encoding.Unicode.GetBytes(userInfo.Password);
                userInfo.Password = Convert.ToBase64String(pwdBytes);
                User newUser = _mapper.Map<UserRegisterDTO, User>(userInfo);
                await _unitOfWork.UserRepository.Create(newUser);
                _unitOfWork.Save();
                UserDTO returnUser = _mapper.Map<User, UserDTO>(newUser);
                return returnUser;
            }
        }

        public async Task<UserDTO> EditUserInfo(UserEditDTO userInfo)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userInfo.UserId);
                user.Name = userInfo.Name;
                user.LastName = userInfo.LastName;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                UserDTO returnUser = _mapper.Map<User, UserDTO>(user);
                return returnUser;
            }
        }

        public async Task<bool> ChangePassword(UserChangePassDTO userInfo)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userInfo.UserId);
                byte[] oldPwdBytes = System.Text.Encoding.Unicode.GetBytes(userInfo.OldPassword);
                userInfo.OldPassword = Convert.ToBase64String(oldPwdBytes);
                
                if (user.Password != userInfo.OldPassword)
                    return false;

                byte[] newPwdBytes = System.Text.Encoding.Unicode.GetBytes(userInfo.NewPassword);
                user.Password = Convert.ToBase64String(newPwdBytes);
                _unitOfWork.UserRepository.Update(user);

                return _unitOfWork.Save();
            }
        }

        public async Task<bool> DeletePicture(int userID)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userID);
                user.Picture = null;
                return true;
            }
        }
    }
}
