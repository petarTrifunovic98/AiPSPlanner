using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DataAccess.Entities;

namespace TravelPlan.Contracts.RepositoryContracts
{
    public interface IItemRepository: IRepositoryBase<Item>
    {
        Task<IEnumerable<Item>> GetTripItems(int tripId);
        Task<Item> GetSpecificItemWithUserAndTrip(int itemId);
        Task<IEnumerable<Item>> GetUserTripItems(int userId, int tripId);
        Task<IEnumerable<Item>> GetUserItems(int userId);
    }
}
