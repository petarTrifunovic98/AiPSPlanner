﻿using AutoMapper;
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

namespace TravelPlan.Services.BusinessLogicServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<UserAuthenticateResponseDTO> AddUserAccount(UserRegisterDTO userInfo)
        {
            using (_unitOfWork)
            {
                if (_unitOfWork.UserRepository.UsernameTaken(userInfo.Username))
                    return null;

                byte[] pwdBytes = Encoding.Unicode.GetBytes(userInfo.Password);
                userInfo.Password = Convert.ToBase64String(pwdBytes);
                User newUser = _mapper.Map<UserRegisterDTO, User>(userInfo);
                await _unitOfWork.UserRepository.Create(newUser);
                await _unitOfWork.Save();
                UserAuthenticateResponseDTO returnUser = _mapper.Map<User, UserAuthenticateResponseDTO>(newUser);
                returnUser.Token = GenerateToken(newUser);
                return returnUser;
            }
        }

        public async Task<UserAuthenticateResponseDTO> LogUserIn(UserLoginDTO userInfo)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.GetUserByUsername(userInfo.Username);
                byte[] pwdBytes = Encoding.Unicode.GetBytes(userInfo.Password);
                userInfo.Password = Convert.ToBase64String(pwdBytes);
                if (user.Password != userInfo.Password)
                    return null;

                UserAuthenticateResponseDTO returnUser = _mapper.Map<User, UserAuthenticateResponseDTO>(user);
                returnUser.Token = GenerateToken(user);
                return returnUser;
            }
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
                UserDTO returnUser = _mapper.Map<User, UserDTO>(user);
                return returnUser;
            }
        }

        public async Task<bool> ChangePassword(UserChangePassDTO userInfo)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userInfo.UserId);
                byte[] oldPwdBytes = Encoding.Unicode.GetBytes(userInfo.OldPassword);
                userInfo.OldPassword = Convert.ToBase64String(oldPwdBytes);

                if (user.Password != userInfo.OldPassword)
                    return false;

                byte[] newPwdBytes = Encoding.Unicode.GetBytes(userInfo.NewPassword);
                user.Password = Convert.ToBase64String(newPwdBytes);
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
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetSpecificUser(int userId)
        {
            using(_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                return _mapper.Map<User, UserDTO>(user);
            }
            throw new NotImplementedException();
        }
    }
}
