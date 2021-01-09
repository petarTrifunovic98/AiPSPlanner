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
    [Route("api/location")]
    public class LocationController: ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost]
        [Route("create-location")]
        public async Task<ActionResult> CreateLocation(LocationCreateDTO newLocation)
        {
            try
            {
                LocationDTO result = await _locationService.CreateLocation(newLocation);
                if (result != null)
                    return Ok(result);
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-location/{locationId}")]
        public ActionResult DeleteLocation(int locationId)
        {
            try
            {
                _locationService.DeleteLocation(locationId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-location")]
        public async Task<ActionResult> EditLocation(LocationEditDTO locationInfo)
        {
            try
            {
                LocationDTO result = await _locationService.EditLocationInfo(locationInfo);
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
