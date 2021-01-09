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
    [Route("api/accommodation")]
    public class AccommodationController : ControllerBase
    {
        private readonly IAccommodationService _accommodationService;
        public AccommodationController(IAccommodationService accommodationService)
        {
            _accommodationService = accommodationService;
        }

        [HttpPost]
        [Route("create-accommodation")]
        public async Task<ActionResult> CreateAccommodation(AccommodationCreateDTO newAccommodation)
        {
            try
            {
                AccommodationDTO result = await _accommodationService.CreateAccommodation(newAccommodation);
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
        [Route("delete-accommodation/{accommodationId}")]
        public ActionResult DeleteAccommodation(int accommodationId)
        {
            try
            {
                _accommodationService.DeleteAccommodation(accommodationId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-accommodation")]
        public async Task<ActionResult> EditAccommodation(AccommodationEditDTO accommodationInfo)
        {
            try
            {
                AccommodationDTO result = await _accommodationService.EditAccommodationInfo(accommodationInfo);
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
