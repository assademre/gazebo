using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository notificationRepository) 
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNotification()
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var notifications = await _notificationRepository.GetTaskNotifications(userId);


            if (notifications == null)
            {
                return BadRequest($"Notification was not found.");
            }

            return Ok(notifications);
        }

        [HttpPut()]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MakeNotificationRead([FromBody] int notificationId)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var result = await _notificationRepository.MakeNotificationRead(userId, notificationId);


            if (!result)
            {
                return BadRequest($"Notification was not found.");
            }

            return Ok(result);
        }

        private int GetUser()
        {
            var claim = User.Claims
               .FirstOrDefault(x => x.Type == "userId");

            if (claim == null)
            {
                return 0;
            }
            var userIdString = claim?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return 0;
            }

            return userId;
        }
    }
}
