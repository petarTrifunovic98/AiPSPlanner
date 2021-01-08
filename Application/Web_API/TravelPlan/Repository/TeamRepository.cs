using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(TravelPlanDbContext context) : base(context)
        { }
        public async Task<IEnumerable<Team>> GetTeamsWithMembers()
        {
            IEnumerable<Team> teams = await _dbSet.Include(team => team.Members).ToListAsync();
            return teams;
        }

        public async Task<Team> GetTeamWithMembers(int id)
        {
            Team team = await _dbSet.Include(team => team.Members).FirstOrDefaultAsync(team => team.TeamId == id);
            return team;
        }
    }
}
