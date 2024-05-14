using AutoMapper;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventTaskRepository _eventTaskRepository;
        private readonly IEventMemberRepository _eventMemberRepository;
        public IMapper _mapper;

        public CommentController(
            ICommentRepository commentRepository,
            INotificationRepository notificationRepository,
            IEventTaskRepository eventTaskRepository,
            IEventMemberRepository eventMemberRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _notificationRepository = notificationRepository;
            _eventTaskRepository = eventTaskRepository;
            _eventMemberRepository = eventMemberRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddComment([FromBody] Models.Comment comment)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            if (comment == null)
            {
                return BadRequest("comment is empty");
            }

            var response = await _commentRepository.AddComment(comment);

            if (!response)
            {
                return BadRequest("Encounter an error while adding the comment");
            }

            var notificationResponse = await _notificationRepository.CreateNewCommentNotifications(userId, comment.PostGroupId, comment.PostGroupTypeId);

            if (!notificationResponse)
            {
                return BadRequest("Encounter an error while creating notifications");
            }

            return Ok(response);
        }

        [HttpGet("postgrouptypeid={postGroupTypeId:int}&postgroupid={postGroupId:int}&pagenumber={pageNumber}&pagesize={pageSize}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCommentsByPostGroupId([FromRoute] int postGroupTypeId, [FromRoute] int postGroupId, int pageNumber, int pageSize)
        {
            var userId = GetUser();
            var eventId = 0;

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            if (postGroupId == 0 || postGroupTypeId == 0)
            {
                return BadRequest("The post group not found");
            }

            if (postGroupTypeId == (int)PostGroup.Task)
            {
                eventId = await _eventTaskRepository.GetEventIdByTaskId(postGroupId);
            }

            else if (postGroupTypeId == (int)PostGroup.Event)
            {
                eventId = postGroupId;
            }

            if (eventId == 0)
            {
                return BadRequest("The post group not found");
            }

            var isMember = await _eventMemberRepository.IsUserMember(eventId, userId);

            if (isMember == false)
            {
                return BadRequest("User does not have the permission for this action");
            }

            var commentList = await _commentRepository.GetCommentsByPostGroupId(postGroupTypeId, postGroupId, pageNumber, pageSize);

            if (commentList == null)
            {
                return Ok(new List<CommentDto>());
            }

            return Ok(commentList);
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
