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
    public class ItemRepository : RepositoryBase<Item>, IItemRepository
    {
        public ItemRepository(TravelPlanDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Item>> GetTripItems(int tripId)
        {
            IEnumerable<Item> items = await _dbSet.Select(item => item).Where(item => item.TripId == tripId)
                                                  .Include(item => item.Trip).Include(item => item.User).ToListAsync();
            return items;
        }

        public async Task<Item> GetSpecificItemWithUserAndTrip(int itemId)
        {
            Item item = await _dbSet.Include(item => item.Trip).Include(item => item.User)
                                    .FirstOrDefaultAsync(item => item.ItemId == itemId);
            return item;
        }

        public async Task<IEnumerable<Item>> GetUserTripItems(int userId, int tripId)
        {
            IEnumerable<Item> items = await _dbSet.Select(item => item).Where(item => item.UserId == userId && item.TripId == tripId)
                                                     .Include(item => item.Trip).Include(item => item.User).ToListAsync();
            return items;
        }

        public async Task<IEnumerable<Item>> GetUserItems(int userId)
        {
            IEnumerable<Item> items = await _dbSet.Select(item => item).Where(item => item.UserId == userId)
                                                  .Include(item => item.Trip).Include(item => item.User).ToListAsync();
            return items;
        }
    }
}
