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
        Task<VoteDTO> Vote(VoteCreateDTO newVote, int tripId);
        Task<VoteDTO> EditVote(VoteEditDTO voteInfo, int tripId);
        Task<VotableDTO> RemoveVote(int voteId, int tripId);
        Task<ICollection<VoteDTO>> GetVotes(int VotableId, bool positive);
    }
}
