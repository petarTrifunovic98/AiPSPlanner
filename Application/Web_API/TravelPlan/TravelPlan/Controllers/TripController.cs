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
    [Route("api/trip")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IEditRightsService _editRightsService;
        private readonly RedisAppSettings _redisAppSettings;

        public TripController(ITripService tripService, IEditRightsService editRightsService, IOptions<RedisAppSettings> redisAppSettings)
        {
            _tripService = tripService;
            _editRightsService = editRightsService;
            _redisAppSettings = redisAppSettings.Value;
        }

        [HttpPost]
        [Route("creator/{creatorId}/create-trip")]
        public async Task<ActionResult> CreateTrip(int creatorId, [FromBody] TripCreateDTO newTrip)
        {
            try
            {
                TripDTO result = await _tripService.CreateTrip(creatorId, newTrip);
                if (result != null)
                    return Ok(result);
                return BadRequest(new JsonResult("Trip dates are not valid."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-info")]
        public async Task<ActionResult> EditTripInfo([FromBody] TripEditDTO tripInfo)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripInfo.TripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                TripDTO result = await _tripService.EditTripInfo(tripInfo);
                await _editRightsService.ProlongEditRights(tripInfo.TripId, _redisAppSettings.EditRightsProlongedTTL);
                if (result != null)
                    return Ok(result);
                return BadRequest(new JsonResult("Trip dates are not valid."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("remove-user/{tripId}/{userId}")]
        public async Task<ActionResult> RemoveUserFromTrip(int tripId, int userId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
                if (await _tripService.RemoveUserFromTrip(tripId, userId))
                    return Ok();
                return BadRequest(new JsonResult("Before you leave the trip you must delegate to other travelers all of the items you are responsible for."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("add-user/{tripId}/{memberId}")]
        public async Task<ActionResult> AddUserToTrip(int tripId, int memberId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                TripDTO trip = await _tripService.AddMemberToTrip(tripId, memberId, false);
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
                if (trip != null)
                    return Ok(trip);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("add-team/{tripId}/{memberId}")]
        public async Task<ActionResult> AddTeamToTrip(int tripId, int memberId)
        {
            try
            {
                if (!await _editRightsService.HasEditRights(tripId))
                    return BadRequest(new JsonResult("You can't currently edit this trip."));
                TripDTO trip = await _tripService.AddMemberToTrip(tripId, memberId, true);
                await _editRightsService.ProlongEditRights(tripId, _redisAppSettings.EditRightsProlongedTTL);
                if (trip != null)
                    return Ok(trip);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-trip/{tripId}")]
        public async Task<ActionResult> GetSpecificTrip(int tripId)
        {
            try
            {
                TripDTO trip = await _tripService.GetTripWithItemsAndMembers(tripId);
                return Ok(trip);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-trips/user/{userId}")]
        public async Task<ActionResult> GetUserTrips(int userId)
        {
            try
            {
                IEnumerable<TripDTO> trips = await _tripService.GetUserTrips(userId);
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-trip-with-locations/{tripId}")]
        public async Task<ActionResult> GetTripWithLocations(int tripId)
        {
            try
            {
                TripDTO trip = await _tripService.GetTripWithLocations(tripId);
                return Ok(trip);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-trip-additional-info/{tripId}")]
        public async Task<ActionResult> GetTripAdditionalInfo(int tripId)
        {
            try
            {
                TripAdditionalInfoDTO tripInfo = await _tripService.GetTripAdditionalInfo(tripId);
                return Ok(tripInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("add-to-trip-packing-list/{tripId}")]
        public async Task<ActionResult> AddItemToPackingList(int tripId, [FromBody]TripStandardListItemDTO item)
        {
            try
            {
                TripAdditionalInfoDTO tripInfo = await _tripService.AddItemToPackingList(tripId, item);
                return Ok(tripInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-trip-types")]
        public ActionResult GetTripTypes()
        {
            try
            {
                return Ok(_tripService.GetTripTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
