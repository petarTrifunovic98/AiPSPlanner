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
        public async Task<bool> CreateTeam(int userId, CreateTeamDTO newTeam)
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
                return _unitOfWork.Save();
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
    }
}
