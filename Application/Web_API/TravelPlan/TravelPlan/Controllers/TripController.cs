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
    [Route("api/trip")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IEditRightsService _editRightsService;
        public TripController(ITripService tripService, IEditRightsService editRightsService)
        {
            _tripService = tripService;
            _editRightsService = editRightsService;
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
                    return BadRequest("You can't currently edit this trip.");
                if (await _tripService.RemoveUserFromTrip(tripId, userId))
                    return Ok();
                return BadRequest("Before you leave the trip you must delegate to other travelers all of the items you are responsible for.");
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
    }
}
