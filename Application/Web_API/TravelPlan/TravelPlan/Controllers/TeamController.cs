using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.API.Controllers
{
    [ApiController]
    [Route("api/team")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        [Route("creator/{creatorId}/create-team")]
        public async Task<ActionResult> CreateTeam(int creatorId, [FromBody]CreateTeamDTO newTeam)
        {
            try
            {
                TeamDTO result = await _teamService.CreateTeam(creatorId, newTeam);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-info")]
        public async Task<ActionResult> EditTeamInfo([FromBody] TeamEditDTO teamInfo)
        {
            try
            {
                TeamDTO result = await _teamService.EditTeamInfo(teamInfo);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("remove-user/{teamId}/{userId}")]
        public async Task<ActionResult> RemoveUserFromTeam(int teamId, int userId)
        {
            try
            {
                if (await _teamService.RemoveUserFromTeam(teamId, userId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("add-user/{teamId}/{memberId}")]
        public async Task<ActionResult> AddUserToTeam(int teamId, int memberId)
        {
            try
            {
                TeamDTO team = await _teamService.AddMemberToTeam(teamId, memberId, false);
                if (team != null)
                    return Ok(team);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("add-team/{teamId}/{memberId}")]
        public async Task<ActionResult> AddTeamToTeam(int teamId, int memberId)
        {
            try
            {
                TeamDTO team = await _teamService.AddMemberToTeam(teamId, memberId, true);
                if (team != null)
                    return Ok(team);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-teams")]
        public async Task<ActionResult> GetTeams()
        {
            try
            { 
                IEnumerable<TeamDTO> teams = await _teamService.GetTeams();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-team/{teamId}")]
        public async Task<ActionResult> GetSpecificTeam(int teamId)
        {
            try
            {
                TeamDTO team = await _teamService.GetSpecificTeam(teamId);
                return Ok(team);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-teams/user/{userId}")]
        public async Task<ActionResult> GetUserTeams(int userId)
        {
            try
            {
                IEnumerable<TeamDTO> teams = await _teamService.GetUserTeams(userId);
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
