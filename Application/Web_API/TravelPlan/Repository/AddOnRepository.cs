using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.Contracts.RepositoryContracts;
using TravelPlan.DataAccess;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Repository
{
    public class AddOnRepository : RepositoryBase<AddOn>, IAddOnRepository
    {
        public AddOnRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<AddOn> GetAddOnWithVotable(int AddOnId)
        {
            return await _dbSet.Include(addOn => addOn.Votable).Where(addOn => addOn.AddOnId == AddOnId).FirstOrDefaultAsync();
        }
    }
}
