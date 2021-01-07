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
                if (await _teamService.CreateTeam(creatorId, newTeam))
                    return Ok();
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
            IEnumerable<TeamDTO> teams = await _teamService.GetTeams();
            return Ok(teams);
        }
    }
}
