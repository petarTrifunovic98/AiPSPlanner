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
        public TripController(ITripService tripService)
        {
            _tripService = tripService;
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
                return BadRequest("Trip dates are not valid.");
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
                TripDTO result = await _tripService.EditTripInfo(tripInfo);
                if (result != null)
                    return Ok(result);
                return BadRequest("Trip dates are not valid.");
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
                if (await _tripService.RemoveUserFromTrip(tripId, userId))
                    return Ok();
                return BadRequest();
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
    }
}
