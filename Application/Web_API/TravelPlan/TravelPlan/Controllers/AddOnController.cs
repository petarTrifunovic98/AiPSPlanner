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
        public AddOnController(IAddOnService addOnService)
        {
            _addOnService = addOnService;
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
                AddOnDTO result = await _addOnService.CreateAddOn(newAddOn);
                if (result != null)
                    return Ok(result);
                return BadRequest("Invalid add on information");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-add-on")]
        public async Task<ActionResult> EditAddOn([FromBody] AddOnEditDTO addOnInfo)
        {
            try
            {
                AddOnDTO result = await _addOnService.EditAddOn(addOnInfo);
                if (result != null)
                    return Ok(result);
                return BadRequest("Invalid add on information");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
