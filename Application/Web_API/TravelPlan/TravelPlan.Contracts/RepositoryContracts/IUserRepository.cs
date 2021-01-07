using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IUserRepository: IRepositoryBase<User>
    {
        bool UsernameTaken(string username);
    }
}
