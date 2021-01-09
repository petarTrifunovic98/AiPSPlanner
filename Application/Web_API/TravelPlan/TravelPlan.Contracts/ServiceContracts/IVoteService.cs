using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IVoteService
    {
        Task<VoteEditDTO> HaveIVotedFor(int votableId, int userId);
        Task<VoteDTO> Vote(VoteCreateDTO newVote);
        Task<VoteDTO> EditVote(VoteEditDTO voteInfo);
        Task<VotableDTO> RemoveVote(int voteId);
        Task<ICollection<VoteDTO>> GetVotes(int VotableId, bool positive);
    }
}
