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
    [Route("api/add-on")]
    public class AddOnController : ControllerBase
    {
        private readonly IAddOnService _addOnService;
        private readonly IEditRightsService _editRightsService;
        public AddOnController(IAddOnService addOnService, IEditRightsService editRightsService)
        {
            _addOnService = addOnService;
            _editRightsService = editRightsService;
        }

        [HttpGet]
        [Route("get-available-decorations/{tripId}")]
        public async Task<ActionResult> GetAvailableDecorations(int tripId)
        {
            try
            {
                List<DecorationAvailableDTO> result = await _addOnService.GetAvailableDecorations(tripId);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create-add-on")]
        public async Task<ActionResult> CreateAddOn([FromBody]AddOnCreateDTO newAddOn)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(newAddOn.TripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                AddOnDTO result = await _addOnService.CreateAddOn(newAddOn);
                if (result != null)
                    return Ok(result);
                return BadRequest(new JsonResult("Invalid add on information"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-add-on/{tripId}")]
        public async Task<ActionResult> EditAddOn(int tripId, [FromBody] AddOnEditDTO addOnInfo)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                AddOnDTO result = await _addOnService.EditAddOn(addOnInfo, tripId);
                if (result != null)
                    return Ok(result);
                return BadRequest(new JsonResult("Invalid add on information"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-add-on/{addOnId}/{tripId}")]
        public async Task<ActionResult> DeleteAddOn(int addOnId, int tripId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                if (await _addOnService.DeleteAddOn(addOnId, tripId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-add-ons/{tripId}")]
        public async Task<ActionResult> GetTripAddOns(int tripId)
        {
            try
            {
                List<AddOnDTO> result = await _addOnService.GetTripAddOns(tripId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
