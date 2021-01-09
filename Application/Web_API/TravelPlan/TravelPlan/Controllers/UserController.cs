using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DTOs.DTOs;
using TravelPlan.Services;

namespace TravelPlan.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("add-account")]
        public async Task<ActionResult> AddUserAccount([FromBody] UserRegisterDTO userInfo)
        {
            try
            {
                UserAuthenticateResponseDTO result = await _userService.AddUserAccount(userInfo);
                if (result == null)
                    return BadRequest("The desired username is not available.");
                return Ok(result);
            }
            catch(Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("edit-info")]
        public async Task<ActionResult> EditUserInfo([FromBody] UserEditDTO userInfo)
        {
            try
            {
                UserDTO result = await _userService.EditUserInfo(userInfo);
                if (result == null)
                    return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangePassDTO userInfo)
        {
            try
            {
                if (await _userService.ChangePassword(userInfo))
                    return Ok();
                return BadRequest("Incorrect old password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("delete-picture/{userId}")]
        public async Task<ActionResult> DeletePicture(int userId)
        {
            try
            {
                if (await _userService.DeletePicture(userId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
