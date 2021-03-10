using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface ITeamRepository:IRepositoryBase<Team>
    {
        Task<IEnumerable<Team>> GetTeamsWithMembers();
        Task<Team> GetTeamWithMembers(int id);
        Task<IEnumerable<Team>> GetUserTeamsWithMembers(User user); 
    }
}
