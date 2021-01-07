using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts.ServiceContracts;
using TravelPlan.DTOs.DTOs;

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
        public async Task<ActionResult> AddUserAccount(UserRegisterDTO userInfo)
        {
            try
            { 
                if(await _userService.AddUserAccount(userInfo))
                    return Ok();
                return BadRequest();
            }
            catch(Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}
