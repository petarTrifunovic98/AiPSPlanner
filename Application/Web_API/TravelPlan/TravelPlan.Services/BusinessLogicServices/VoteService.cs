using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Services.MessagingService;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class VoteService : IVoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private MessageControllerService _messageControllerService;

        public VoteService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<MessageHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageControllerService = new MessageControllerService(hubContext);
        }

        public async Task<VoteEditDTO> HaveIVotedFor(int votableId, int userId)
        {
            using (_unitOfWork)
            {
                Votable votable = await _unitOfWork.VotableRepository.GetVotableWithVotes(votableId);
                Vote vote = votable.Votes.Where(x => x.UserId == userId).FirstOrDefault();
                if (vote == null)
                    return null;
                return _mapper.Map<Vote, VoteEditDTO>(vote);
            }
        }

        public async Task<VoteDTO> Vote(VoteCreateDTO newVote, int tripId)
        {
            using (_unitOfWork)
            {
                Vote vote = _mapper.Map<VoteCreateDTO, Vote>(newVote);
                Votable votable = await _unitOfWork.VotableRepository.GetVotableWithVotes(newVote.VotableId);

                if (votable.Votes.Where(x => x.UserId == newVote.UserId).FirstOrDefault() != null)
                    return null;

                votable.Votes.Add(vote);
                if (newVote.Positive)
                    votable.PositiveVotes++;
                else
                    votable.NegativeVotes++;
                vote.Votable = votable;

                _unitOfWork.VotableRepository.Update(votable);
                await _unitOfWork.VoteRepository.Create(vote);
                await _unitOfWork.Save();

                await _messageControllerService.NotifyOnTripChanges(tripId, "ChangeVotable", _mapper.Map<Votable, VotableDTO>(votable));

                return _mapper.Map<Vote, VoteDTO>(vote);
            }
        }

        public async Task<VoteDTO> EditVote(VoteEditDTO voteInfo, int tripId)
        {
            using (_unitOfWork)
            {
                Vote vote = await _unitOfWork.VoteRepository.FindByID(voteInfo.VoteId);

                if (vote.Positive != voteInfo.Positive)
                {
                    Votable votable = await _unitOfWork.VotableRepository.FindByID(vote.VotableId);
                    if (vote.Positive)
                    {
                        votable.NegativeVotes++;
                        votable.PositiveVotes--;
                    }
                    else
                    {
                        votable.NegativeVotes--;
                        votable.PositiveVotes++;
                    }

                    await _messageControllerService.NotifyOnTripChanges(tripId, "ChangeVotable", _mapper.Map<Votable, VotableDTO>(votable));
                }

                vote.Positive = voteInfo.Positive;
                await _unitOfWork.Save();

                return _mapper.Map<Vote, VoteDTO>(vote);
            }
        }

        public async Task<VotableDTO> RemoveVote(int voteId, int tripId)
        {
            using (_unitOfWork)
            {
                Vote vote = await _unitOfWork.VoteRepository.FindByID(voteId);
                Votable votable = await _unitOfWork.VotableRepository.GetVotableWithVotes(vote.VotableId);

                votable.Votes.Remove(vote);
                if (vote.Positive)
                    votable.PositiveVotes--;
                else
                    votable.NegativeVotes--;

                _unitOfWork.VotableRepository.Update(votable);
                _unitOfWork.VoteRepository.Delete(voteId);
                await _unitOfWork.Save();

                await _messageControllerService.NotifyOnTripChanges(tripId, "ChangeVotable", _mapper.Map<Votable, VotableDTO>(votable));

                return _mapper.Map<Votable, VotableDTO>(votable);
            }
        }

        public async Task<ICollection<VoteDTO>> GetVotes(int VotableId, bool positive)
        {
            using (_unitOfWork)
            {
                Votable votable = await _unitOfWork.VotableRepository.GetVotableWithVotes(VotableId);

                List<VoteDTO> retValue = votable.Votes.Where(vote => vote.Positive == positive)
                                                      .Select(vote => _mapper.Map<Vote, VoteDTO>(vote)).ToList();
                return retValue;
            }
        }
    }
}
