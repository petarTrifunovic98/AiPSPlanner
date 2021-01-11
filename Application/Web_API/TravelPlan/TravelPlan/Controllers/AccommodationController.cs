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
                return BadRequest("Accommodation dates are not valid.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-accommodation/{accommodationId}")]
        public async Task<ActionResult> DeleteAccommodation(int accommodationId)
        {
            try
            {
                await _accommodationService.DeleteAccommodation(accommodationId);
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
                return BadRequest("Accommodation dates are not valid.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-accommodation/{accommodationId}")]
        public async Task<ActionResult> GetAccommodation(int accommodationId)
        {
            try
            {
                return Ok(await _accommodationService.GetSpecificAccommodation(accommodationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-location-accommodations/{locationId}")]
        public async Task<ActionResult> GetLocationAccommodations(int locationId)
        {
            try
            {
                return Ok(await _accommodationService.GetAccommodationsForLocation(locationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("add-picture")]
        public async Task<ActionResult> AddAccommodationPicture(AccommodationPictureCreateDTO picture)
        {
            try
            {
                AccommodationPictureDTO accommodationPicture = await _accommodationService.AddAccommodationPicture(picture);
                return Ok(accommodationPicture);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-pictures/accommodation/{accommodationId}")]
        public async Task<ActionResult> GetAccommodationPictures(int accommodationId)
        {
            try
            {
                IEnumerable<AccommodationPictureDTO> pictures = await _accommodationService.GetAccommodationPictures(accommodationId);
                return Ok(pictures);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-picture/{pictureId}")]
        public async Task<ActionResult> DeleteAccommodationPicture(int pictureId)
        {
            try
            {
                await _accommodationService.DeleteAccommodationPicture(pictureId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
