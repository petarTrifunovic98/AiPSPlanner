﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface ITeamService
    {
        Task<TeamDTO> CreateTeam(int userId, CreateTeamDTO newTeam);
        Task<TeamDTO> EditTeamInfo(TeamEditDTO teamInfo);
        Task<bool> RemoveUserFromTeam(int teamId, int userId);
        Task<TeamDTO> AddMemberToTeam(int teamId, int memberId, bool IsTeam);
        Task<IEnumerable<TeamDTO>> GetTeams();
        Task<TeamDTO> GetSpecificTeam(int teamId);
        Task<IEnumerable<TeamDTO>> GetUserTeams(int userId);
    }
}
