using Gazebo.Interfaces;
using Gazebo.Models.Enums;
using Gazebo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/friendship")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly INotificationRepository _notificationRepository;
        public FriendshipController(IFriendshipRepository friendshipRepository, INotificationRepository notificationRepository) 
        {
            _friendshipRepository = friendshipRepository;
            _notificationRepository = notificationRepository;
        }

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetFriends()
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var friendsList = await _friendshipRepository.GetFriends(userId);

            return Ok(friendsList);
        }
        
        [HttpPut("request/{receiverId}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SendFriendshipRequest([FromRoute] int receiverId)
        {
            var userId = GetUser();
            if (userId == 0 || receiverId == 0)
            {
                return BadRequest("userId or receiverId is wrong");
            }

            var sendResponse = await _friendshipRepository.SendFriendshipRequest(userId, receiverId);

            if (!sendResponse)
            {
                return BadRequest("Something went wrong during send request");
            }

            var friendshipRequestNotification = await _notificationRepository.CreateFriendshipNotification(userId, receiverId);

            if(!friendshipRequestNotification)
            {
                return BadRequest("Something went wrong during notification creation");
            }

            return Ok(sendResponse);
        }
        
        [HttpPut("response/{senderId}/{responseId}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RespondFriendshipRequest([FromRoute] int senderId, [FromRoute] int responseId)
        {
            var userId = GetUser();

            if (userId == 0 || senderId == 0 || responseId == 0)
            {
                return BadRequest("userId´, senderId or responseId is wrong");
            }

            var respond = await _friendshipRepository.RespondFriendshipRequest(userId, senderId, responseId);

            if (!respond)
            {
                return BadRequest("Something went wrong during accept request");
            }

            if (responseId == (int)FriendshipStatus.Accepted)
            {
                var notificationResponse = await _notificationRepository.AcceptNotification(senderId, userId);
                if (!notificationResponse)
                {
                    return BadRequest("Something went wrong druing notification cretion");
                }
            }

            return Ok(respond);
        }

        [HttpDelete("{friendId}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveFriend([FromRoute] int friendId)
        {
            var userId = GetUser();

            if (userId == 0 || friendId == 0)
            {
                return BadRequest("userId or senderId is wrong");
            }

            var deleteRespond = await _friendshipRepository.RemoveFriend(userId, friendId);

            if (!deleteRespond)
            {
                return BadRequest("Something went wrong during accept request");
            }

            return Ok(deleteRespond);
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
