using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IUserRepository: IRepositoryBase<User>
    {
        bool UsernameTaken(string username);
        Task<User> GetUserWithItems(int id);
    }
}
