﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Helpers;

namespace TravelPlan.API.Controllers
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IEditRightsService _editRightsService;
        private readonly RedisAppSettings _redisAppSettings;
        public ItemController(IItemService itemService, IEditRightsService editRightsService, IOptions<RedisAppSettings> redisAppSettings)
        {
            _itemService = itemService;
            _editRightsService = editRightsService;
            _redisAppSettings = redisAppSettings.Value;
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
                await _editRightsService.ProlongEditRights(newItem.TripId, _redisAppSettings.EditRightsProlongedTTL);
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
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
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
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
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
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
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
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
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
