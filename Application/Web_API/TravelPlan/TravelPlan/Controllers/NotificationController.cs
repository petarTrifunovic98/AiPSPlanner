using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlan.Contracts.ServiceContracts;

namespace TravelPlan.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("get-notification-number/{userId}")]
        public ActionResult GetNotificationNumber(int userId)
        {
            try
            {
                return Ok(_notificationService.GetNotificationNumber(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-notification/{notificationId}")]
        public async Task<ActionResult> GetNotification(int notificationId)
        {
            try
            {
                return Ok(await _notificationService.GetNotification(notificationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-user-notifications/{userId}")]
        public async Task<ActionResult> GetUserNotifications(int userId)
        {
            try
            {
                return Ok(await _notificationService.GetNotifications(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("seen-notifications/{userId}")]
        public async Task<ActionResult> SeenMyNotifications(int userId)
        {
            try
            {
                return Ok(await _notificationService.SeenMyNotifications(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("seen-a-notification/{notificationId}")]
        public async Task<ActionResult> SeenANotification(int notificationId)
        {
            try
            {
                return Ok(await _notificationService.SeenANotification(notificationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-seen/{userId}/{itemRelated}")]
        public async Task<ActionResult> DeleteMySeenNotifications(int userId, bool itemRelated)
        {
            try
            {
                return Ok(await _notificationService.DeleteSeenNotifications(userId, itemRelated));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
