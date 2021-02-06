using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Services;

namespace TravelPlan.API.Controllers
{
    [ApiController]
    [Route("api/edit-rights")]
    public class EditRightsController : ControllerBase
    {
        private readonly IEditRightsService _editRightsService;

        public EditRightsController(IEditRightsService editRightsService)
        {
            _editRightsService = editRightsService;
        }

        [HttpPost]
        [Route("request-edit/trip/{tripId}/user/{userId}")]
        public async Task<ActionResult> RequestTripEdit(int tripId, int userId)
        {
            try
            {
                bool editRightsGiven = await _editRightsService.RequestTripEdit(tripId, userId);
                return Ok(editRightsGiven);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("release-edit/trip/{tripId}/")]
        public async Task<ActionResult> ReleaseTripEdit(int tripId)
        {
            try
            {
                await _editRightsService.ReleaseTripEdit(tripId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("cancel-edit-request/trip/{tripId}/user/{userId}")]
        public async Task<ActionResult> CancelTripEditRequest(int tripId, int userId)
        {
            try
            {
                await _editRightsService.LeaveRequestEditQueue(tripId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
