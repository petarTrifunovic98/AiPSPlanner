using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DataAccess.Entities;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<TeamDTO> CreateTeam(int userId, CreateTeamDTO newTeam)
        {
            using(_unitOfWork)
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
                _unitOfWork.Save();

                TeamDTO returnTeam = _mapper.Map<Team, TeamDTO>(team);
                return returnTeam;
            }
        }

        public async Task<IEnumerable<TeamDTO>> GetTeams()
        {
            using (_unitOfWork)
            {
                IEnumerable<Team> teams = await _unitOfWork.TeamRepository2.GetTeamsWithMembers();
                IEnumerable<TeamDTO> teamDTOs = _mapper.Map<IEnumerable<Team>, IEnumerable<TeamDTO>>(teams);
                return teamDTOs;
            }
        }

        public async Task<TeamDTO> EditTeamInfo(TeamEditDTO teamInfo)
        {
            using(_unitOfWork)
            {
                Team team = await _unitOfWork.TeamRepository2.GetTeamWithMembers(teamInfo.TeamId);
                team.Name = teamInfo.Name;
                _unitOfWork.TeamRepository2.Update(team);
                _unitOfWork.Save();
                TeamDTO returnTeam = _mapper.Map<Team, TeamDTO>(team);
                return returnTeam;
            }
        }

        public async Task<bool> RemoveUserFromTeam(int teamId, int userId)
        {
            using (_unitOfWork)
            {
                Team team = await _unitOfWork.TeamRepository2.GetTeamWithMembers(teamId);
                User user = await _unitOfWork.UserRepository.FindByID(userId);
                if(team.Members != null && team.Members.Contains(user))
                {
                    team.Members.Remove(user);
                    user.MyTeams.Remove(team);

                    _unitOfWork.TeamRepository2.Update(team);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    return true;
                }

                return false;
            }
        }
    }
}
