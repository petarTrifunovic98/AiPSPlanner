using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IUserService
    {
        Task<bool> AddUserAccount(UserRegisterDTO userInfo);
    }
}
