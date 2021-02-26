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
    [Route("api/vote")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly IEditRightsService _editRightsService;
        public VoteController(IVoteService voteService, IEditRightsService editRightsService)
        {
            _voteService = voteService;
            _editRightsService = editRightsService;
        }

        [HttpGet]
        [Route("check-voted/{votableId}/{userId}")]
        public async Task<ActionResult> CheckIfVoted(int votableId, int userId)
        {
            try
            {
                VoteEditDTO retValue = await _voteService.HaveIVotedFor(votableId, userId);
                if(retValue == null)
                    return Ok(false);
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("vote/{tripId}")]
        public async Task<ActionResult> Vote(int tripId, [FromBody] VoteCreateDTO newVote)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                VoteDTO retValue = await _voteService.Vote(newVote, tripId);
                if (retValue == null)
                    return BadRequest(new JsonResult("You already voted for this item"));
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("change-vote/{tripId}")]
        public async Task<ActionResult> ChangeVote(int tripId, [FromBody] VoteEditDTO voteInfo)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                VoteDTO retValue = await _voteService.EditVote(voteInfo, tripId);
                if (retValue == null)
                    return BadRequest(new JsonResult("Vote does not exist"));
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("remove-vote/{voteId}/{tripId}")]
        public async Task<ActionResult> RemoveVote(int voteId, int tripId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                VotableDTO retValue = await _voteService.RemoveVote(voteId, tripId);
                if (retValue == null)
                    return BadRequest(new JsonResult("Vote does not exist"));
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-votes/{votableId}/{positive}")]
        public async Task<ActionResult> GetVotes(int votableId, bool positive)
        {
            try
            {
                ICollection<VoteDTO> retValue = await _voteService.GetVotes(votableId, positive);
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
