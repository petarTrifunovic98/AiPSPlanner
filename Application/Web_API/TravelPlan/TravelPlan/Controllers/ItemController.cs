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
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        [Route("create-item")]
        public async Task<ActionResult> CreateItem([FromBody] ItemCreateDTO newItem)
        {
            try
            {
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
        [Route("delete-item/{itemId}")]
        public async Task<ActionResult> DeleteItem(int itemId)
        {
            try
            {
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
        [Route("edit-item")]
        public async Task<ActionResult> EditItem([FromBody] ItemEditDTO itemInfo)
        {
            try
            {
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
        [Route("change-user/{itemId}/{newUserId}")]
        public async Task<ActionResult> ChangeUser(int itemId, int newUserId)
        {
            try
            {
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
        [Route("change-checked/{itemId}")]
        public async Task<ActionResult> ChangeChecked(int itemId)
        {
            try
            {
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
    }
}
