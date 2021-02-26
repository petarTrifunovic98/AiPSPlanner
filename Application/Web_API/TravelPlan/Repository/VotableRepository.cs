using System;
using System.Collections.Generic;
using System.Text;
using TravelPlan.DataAccess.Entities;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TravelPlan.Repository
{
    public class VotableRepository : RepositoryBase<Votable>, IVotableRepository
    {
        public VotableRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<Votable> GetVotableWithVotes(int id)
        {
            Votable votable = await _dbSet.Include(votable => votable.Votes)
                                          .ThenInclude(vote => vote.User)
                                          .FirstOrDefaultAsync(votable => votable.VotableId == id);
            return votable;
        }
    }
}
