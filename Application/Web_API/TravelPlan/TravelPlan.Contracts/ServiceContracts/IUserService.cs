using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IUserService
    {
        Task<UserAuthenticateResponseDTO> AddUserAccount(UserRegisterDTO userInfo);
        Task<UserDTO> EditUserInfo(UserEditDTO userInfo);
        Task<bool> ChangePassword(UserChangePassDTO userInfo);
        Task<bool> DeletePicture(int userID);
    }
}
