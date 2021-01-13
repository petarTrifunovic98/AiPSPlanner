using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogUserIn([FromBody] UserLoginDTO userInfo)
        {
            try
            {
                UserAuthenticateResponseDTO result = await _userService.LogUserIn(userInfo);
                if (result == null)
                    return BadRequest("Wrong password");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpGet]
        [Route("get-users")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                IEnumerable<UserDTO> users = await _userService.GetUsers();
                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-user/{userId}")]
        public async Task<ActionResult> GetSpecificUser(int userId)
        {
            try
            {
                UserDTO user = await _userService.GetSpecificUser(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("change-password-temp")]
        public async Task<ActionResult> ChangePasswordTemp(UserChangePassDTO userInfo)
        {
            try
            {
                await _userService.ChangePasswordTemp(userInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
