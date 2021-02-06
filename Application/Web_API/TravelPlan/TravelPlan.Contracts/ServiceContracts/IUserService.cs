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
        Task<UserAuthenticateResponseDTO> LogUserIn(UserLoginDTO userInfo);
        Task<bool> LogUserOut(int userId);
        Task<UserDTO> EditUserInfo(UserEditDTO userInfo);
        Task<bool> ChangePassword(UserChangePassDTO userInfo);
        Task<bool> DeletePicture(int userID);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetSpecificUser(int userId);
        Task<UserBasicDTO> GetUserByUsername(String username);
        Task<bool> RequestTripEdit(int tripId, int userId);
        Task ReleaseTripEdit(int tripId);
        Task LeaveRequestEditQueue(int tripId, int userId);
    }
}
