using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DTOs.DTOs;

namespace TravelPlan.API.Controllers
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IEditRightsService _editRightsService;
        public ItemController(IItemService itemService, IEditRightsService editRightsService)
        {
            _itemService = itemService;
            _editRightsService = editRightsService;
        }

        [HttpPost]
        [Route("create-item")]
        public async Task<ActionResult> CreateItem([FromBody] ItemCreateDTO newItem)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(newItem.TripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                ItemDTO result = await _itemService.CreateItem(newItem);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-item/{itemId}/{tripId}")]
        public async Task<ActionResult> DeleteItem(int itemId, int tripId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                if (await _itemService.DeleteItem(itemId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-item/{tripId}")]
        public async Task<ActionResult> EditItem(int tripId, [FromBody] ItemEditDTO itemInfo)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                ItemDTO result = await _itemService.EditItemInfo(itemInfo);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("change-user/{itemId}/{newUserId}/{tripId}")]
        public async Task<ActionResult> ChangeUser(int itemId, int newUserId, int tripId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                ItemDTO result = await _itemService.ChangeUser(itemId, newUserId);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("change-checked/{itemId}/{tripId}")]
        public async Task<ActionResult> ChangeChecked(int itemId, int tripId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                ItemDTO result = await _itemService.un_checkItem(itemId);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-items/trip/{tripId}")]
        public async Task<ActionResult> GetTripItems(int tripId)
        {
            try
            {
                IEnumerable<ItemDTO> items = await _itemService.GetTripItems(tripId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-item/{itemId}")]
        public async Task<ActionResult> GetSpecificItem(int itemId)
        {
            try
            {
                ItemDTO item = await _itemService.GetSpecificItem(itemId);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-items/user/{userId}/trip/{tripId}")]
        public async Task<ActionResult> GetUserTripItems(int userId, int tripId)
        {
            try
            {
                IEnumerable<ItemDTO> items = await _itemService.GetUserTripItems(userId, tripId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-items/user/{userId}")]
        public async Task<ActionResult> GetUserItems(int userId)
        {
            try
            {
                IEnumerable<ItemDTO> items = await _itemService.GetUserItems(userId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
