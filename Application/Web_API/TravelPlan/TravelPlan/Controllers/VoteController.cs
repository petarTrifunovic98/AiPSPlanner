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
        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
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
        [Route("vote")]
        public async Task<ActionResult> Vote([FromBody] VoteCreateDTO newVote)
        {
            try
            {
                VoteDTO retValue = await _voteService.Vote(newVote);
                if (retValue == null)
                    return BadRequest("You already voted for this item");
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("change-vote")]
        public async Task<ActionResult> ChangeVote([FromBody] VoteEditDTO voteInfo)
        {
            try
            {
                VoteDTO retValue = await _voteService.EditVote(voteInfo);
                if (retValue == null)
                    return BadRequest("Vote does not exist");
                return Ok(retValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("remove-vote/{voteId}")]
        public async Task<ActionResult> RemoveVote(int voteId)
        {
            try
            {
                VotableDTO retValue = await _voteService.RemoveVote(voteId);
                if (retValue == null)
                    return BadRequest("Vote does not exist");
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
