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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(TravelPlanDbContext context):base(context)
        { }
        public bool UsernameTaken(string username)
        {
            return _dbSet.Any(u => u.Username == username);
        }

        public async Task<User> GetUserWithItems(int id)
        {
            User user = await _dbSet.Include(user => user.MyItems).FirstOrDefaultAsync(user => user.UserId == id);
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User user = await _dbSet.FirstOrDefaultAsync(user => user.Username == username);
            return user;
        }
    }
}
