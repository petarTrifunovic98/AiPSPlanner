using Microsoft.AspNetCore.Mvc;
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
    [Route("api/location")]
    public class LocationController: ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IEditRightsService _editRightsService;
        private readonly RedisAppSettings _redisAppSettings;

        public LocationController(ILocationService locationService, IEditRightsService editRightsService, IOptions<RedisAppSettings> redisAppSettings)
        {
            _locationService = locationService;
            _editRightsService = editRightsService;
            _redisAppSettings = redisAppSettings.Value;
        }

        [HttpPost]
        [Route("create-location")]
        public async Task<ActionResult> CreateLocation([FromBody]LocationCreateDTO newLocation)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(newLocation.TripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                LocationDTO result = await _locationService.CreateLocation(newLocation);
                await _editRightsService.ProlongEditRights(newLocation.TripId, _redisAppSettings.EditRightsProlongedTTL);
                if (result != null)
                    return Ok(result);
                return BadRequest(new JsonResult("Location dates are not valid"));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-location/{locationId}/{tripId}")]
        public async Task<ActionResult> DeleteLocation(int locationId, int tripId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                await _locationService.DeleteLocation(locationId);
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-location/{tripId}")]
        public async Task<ActionResult> EditLocation(int tripId, [FromBody]LocationEditDTO locationInfo)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                LocationDTO result = await _locationService.EditLocationInfo(locationInfo);
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
                if (result != null)
                    return Ok(result);
                return BadRequest(new JsonResult("Location dates are not valid"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-location/{locationId}")]
        public async Task<ActionResult> GetAccommodation(int locationId)
        {
            try
            {
                return Ok(await _locationService.GetSpecificLocation(locationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-trip-locations/{tripId}")]
        public async Task<ActionResult> GetLocationAccommodations(int tripId)
        {
            try
            {
                return Ok(await _locationService.GetLocationsForTrip(tripId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
