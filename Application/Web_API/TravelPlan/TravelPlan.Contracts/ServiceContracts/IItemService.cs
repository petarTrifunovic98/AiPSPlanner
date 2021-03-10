using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.Contracts.ServiceContracts
{
    public interface IItemService
    {
        Task<ItemDTO> CreateItem(ItemCreateDTO newItem);
        Task<bool> DeleteItem(int itemId);
        Task<ItemDTO> EditItemInfo(ItemEditDTO itemInfo);
        Task<ItemDTO> ChangeUser(int ItemId, int newUserId);
        Task<ItemDTO> un_checkItem(int ItemId);
        Task<IEnumerable<ItemDTO>> GetTripItems(int tripId);
        Task<ItemDTO> GetSpecificItem(int itemId);
        Task<IEnumerable<ItemDTO>> GetUserTripItems(int userId, int tripId);
        Task<IEnumerable<ItemDTO>> GetUserItems(int userId);
    }
}
