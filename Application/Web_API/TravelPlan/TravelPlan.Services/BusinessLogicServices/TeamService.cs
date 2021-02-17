using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Services.MessagingService;
using Microsoft.AspNetCore.SignalR;

namespace TravelPlan.Services.BusinessLogicServices
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private MessageControllerService _messageControllerService;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<MessageHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _messageControllerService = new MessageControllerService(hubContext);
        }
        public async Task<TeamDTO> CreateTeam(int userId, CreateTeamDTO newTeam)
        {
            using (_unitOfWork)
            {
                Team team = _mapper.Map<CreateTeamDTO, Team>(newTeam);
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                if (user.MyTeams == null)
                    user.MyTeams = new List<Team>();
                user.MyTeams.Add(team);

                if (team.Members == null)
                    team.Members = new List<User>();
                team.Members.Add(user);

                await _unitOfWork.TeamRepository.Create(team);
                await _unitOfWork.Save();

                TeamDTO returnTeam = _mapper.Map<Team, TeamDTO>(team);
                return returnTeam;
            }
        }

        public async Task<TeamDTO> EditTeamInfo(TeamEditDTO teamInfo)
        {
            using (_unitOfWork)
            {
                Team team = await _unitOfWork.TeamRepository.GetTeamWithMembers(teamInfo.TeamId);
                team.Name = teamInfo.Name;
                _unitOfWork.TeamRepository.Update(team);
                await _unitOfWork.Save();
                TeamDTO returnTeam = _mapper.Map<Team, TeamDTO>(team);
                await _messageControllerService.NotifyOnTeamChanges(teamInfo.TeamId, "EditTeamName", _mapper.Map<Team, TeamEditDTO>(team));
                return returnTeam;
            }
        }

        public async Task<bool> RemoveUserFromTeam(int teamId, int userId)
        {
            using (_unitOfWork)
            {
                Team team = await _unitOfWork.TeamRepository.GetTeamWithMembers(teamId);
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                if (team.Members != null && team.Members.Contains(user))
                {
                    team.Members.Remove(user);
                    user.MyTeams.Remove(team);

                    _unitOfWork.TeamRepository.Update(team);
                    _unitOfWork.UserRepository.Update(user);

                    if (team.Members.Count == 0)
                        _unitOfWork.TeamRepository.Delete(teamId);

                    await _unitOfWork.Save();
                    await _messageControllerService.NotifyOnTeamChanges(teamId, "RemoveUserFromTeam", userId);

                    return true;
                }

                return false;
            }
        }

        public async Task<TeamDTO> AddMemberToTeam(int teamId, int memberId, bool IsTeam)
        {
            using (_unitOfWork)
            {
                Team team = await _unitOfWork.TeamRepository.GetTeamWithMembers(teamId);
                Member member;
                if (IsTeam)
                    member = await _unitOfWork.TeamRepository.GetTeamWithMembers(memberId);
                else
                    member = await _unitOfWork.UserRepository.FindByID(memberId);

                if (team.Members == null)
                    team.Members = new List<User>();

                foreach (User user in member.GetUsers())
                {
                    if (!team.Members.Contains(user))
                    {
                        team.Members.Add(user);
                        if (user.MyTeams == null)
                            user.MyTeams = new List<Team>();
                        user.MyTeams.Add(team);
                        _unitOfWork.UserRepository.Update(user);
                        await _messageControllerService.SendNotification(user.UserId, "AddToTeamNotification", _mapper.Map<Team, TeamDTO>(team));
                    }
                }
                _unitOfWork.TeamRepository.Update(team);
                await _unitOfWork.Save();

                TeamDTO retTeam = _mapper.Map<Team, TeamDTO>(team);
                await _messageControllerService.NotifyOnTeamChanges(teamId, "AddMemberToTeam", 
                                                    member.GetUsers().Select(user => _mapper.Map<User, UserBasicDTO>(user)));
                return retTeam;
            }
        }

        public async Task<IEnumerable<TeamDTO>> GetTeams()
        {
            using (_unitOfWork)
            {
                IEnumerable<Team> teams = await _unitOfWork.TeamRepository.GetTeamsWithMembers();
                IEnumerable<TeamDTO> teamDTOs = _mapper.Map<IEnumerable<Team>, IEnumerable<TeamDTO>>(teams);
                return teamDTOs;
            }
        }

        public async Task<TeamDTO> GetSpecificTeam(int teamId)
        {
            using (_unitOfWork)
            {
                Team team = await _unitOfWork.TeamRepository.GetTeamWithMembers(teamId);
                return _mapper.Map<Team, TeamDTO>(team);
            }
        }

        public async Task<IEnumerable<TeamDTO>> GetUserTeams(int userId)
        {
            using (_unitOfWork)
            {
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                IEnumerable<Team> teams = await _unitOfWork.TeamRepository.GetUserTeamsWithMembers(user);
                IEnumerable<TeamDTO> result = _mapper.Map<IEnumerable<Team>, IEnumerable<TeamDTO>>(teams);
                return result;
            }
        }
    }
}
