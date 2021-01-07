using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface ITeamService
    {
        Task<bool> CreateTeam(int userId, CreateTeamDTO newTeam);
        Task<IEnumerable<TeamDTO>> GetTeams(); 
    }
}
