using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Gazebo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EventOrganizationApp.Controller
{
    [Route("api/event-task")]
    [ApiController]
    public class EventTaskController : ControllerBase
    {
        private readonly IEventTaskRepository _eventTaskRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventMemberRepository _eventMemberRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        public IMapper _mapper;

        public EventTaskController(IEventTaskRepository eventTaskRepository,
            IEventRepository eventRepository,
            IEventMemberRepository eventMemberRepository,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _eventTaskRepository = eventTaskRepository;
            _eventRepository = eventRepository;
            _eventMemberRepository = eventMemberRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }


        [HttpGet("all-tasks")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllUserTasks()
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var allTasks = await _eventTaskRepository.GetAllUserTasks(userId);
            var mappedList = _mapper.Map<IList<EventTaskDto>>(allTasks);

            foreach (var taskDto in mappedList)
            {
                var taskEvent = await _eventRepository.GetEventByEventId(taskDto.EventId);
                taskDto.EventName = taskEvent.EventName;
            }

            if (mappedList.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedList);
        }

        [HttpGet("eventid={eventId:int}/event-task")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<EventTaskDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserTasksForAnEvent([FromRoute] int eventId)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var userEventTasks = await _eventTaskRepository.GetUserTasksForAnEvent(userId, eventId);
            var mappedeventTasks = _mapper.Map<IList<EventTaskDto>>(userEventTasks);

            if (mappedeventTasks.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedeventTasks);
        }

        [HttpGet("{eventId:int}/tasks")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<EventTaskDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTasksForEvent([FromRoute] int eventId)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var isUserMember = await _eventMemberRepository.IsUserMember(eventId, userId);

            if (!isUserMember)
            {
                return BadRequest("User does not have a permission for this action");
            }

            var tasksList = await _eventTaskRepository.GetTasksForEvent(eventId);
            var mappedList = _mapper.Map<IList<EventTaskDto>>(tasksList);

            if (mappedList.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedList);
        }

        [HttpGet("{taskId:int}/task")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTask([FromRoute] int taskId)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var taskInfo = await _eventTaskRepository.GetTask(taskId);

            var isUserMember = await _eventMemberRepository.IsUserMember(taskInfo.EventId, userId);

            if (!isUserMember)
            {
                return BadRequest("User has no access to this task");
            }

            var mappedTaskInfo = _mapper.Map<EventTaskDto>(taskInfo);

            var taskEvent = await _eventRepository.GetEventByEventId(mappedTaskInfo.EventId);

            mappedTaskInfo.EventName = taskEvent.EventName;

            if (mappedTaskInfo == null)
            {
                return NotFound();
            }

            return Ok(mappedTaskInfo);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTask([FromBody] EventTaskDto task)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var isUserAdmin = await _eventMemberRepository.IsUserAdmin(task.EventId, userId);
            
            var mappedTask = _mapper.Map<EventsTask>(task);

            if (!isUserAdmin)
            {
                return BadRequest("The user has no permission for this action.");
            }

            var result = await _eventTaskRepository.CreateTask(mappedTask);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while creating the task");
            }

            var isUserAdded = await _eventMemberRepository.IsUserMember(task.EventId, task.OwnerId);

            if (!isUserAdded)
            {
                var eventMember = new EventMember
                {
                    EventId = task.EventId,
                    UserId = task.OwnerId,
                    IsAdmin = false // for now it's false. We will have add admin option later.
                };

                await _eventMemberRepository.AddEventMember(eventMember);
            }

            var eventInfo = await _eventRepository.GetEventByEventId(task.EventId);

            var createrName = _userRepository.GetUserInfo(userId).Username;

            var notificationResponse = await _notificationRepository.CreateNewTaskNotification(task.OwnerId, createrName, eventInfo.EventName);

            if (!notificationResponse)
            {
                return BadRequest("Encounter an error while creating the notification");
            }

            return Ok("Succesfully created!");
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditTask([FromBody] EventTaskDto task)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var isUserAdmin = await _eventMemberRepository.IsUserAdmin(task.EventId, task.OwnerId);

            if (!(isUserAdmin || userId == task.OwnerId))
            {
                return BadRequest("User does not have a permission for this action.");
            }

            var mappedTask = _mapper.Map<EventsTask>(task);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventTaskRepository.UpdateTask(mappedTask);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while updating the task");
            }

            return Ok("Succesfully updated!");
        }

        // TODO: Do we still need this?
        [HttpPut("{taskId:int}/status")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeTaskStatus([FromRoute] int taskId, [FromQuery] string status)
        {
            Status statusId;
            if (!Enum.TryParse<Status>(status, out statusId))
            {
                ModelState.AddModelError("", "Status does not exist");
            }

            var task = await _eventTaskRepository.GetTask(taskId);
            var mappedTask = _mapper.Map<EventsTask>(task);

            if (mappedTask == null)
            {
                return BadRequest(ModelState);
            }
            mappedTask.StatusId = (int)statusId;
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventTaskRepository.UpdateTask(mappedTask);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while updating the status");
            }

            return Ok("Succesfully updated!");
        }

        [HttpDelete("{taskId:int}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTask([FromRoute] int taskId)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            if (taskId == 0)
            {
                ModelState.AddModelError("", "taskId is wrong");
            }

            var eventId = await _eventTaskRepository.GetEventIdByTaskId(taskId);

            var task = await _eventTaskRepository.GetTask(taskId);


            var isUserAdmin = await _eventMemberRepository.IsUserAdmin(eventId, userId);

            if (!(isUserAdmin || userId == task.OwnerId))
            {
                return BadRequest("User does not have a permission for this action.");
            }

            var mappedTask = _mapper.Map<EventsTask>(task);

            var result = await _eventTaskRepository.DeleteTask(mappedTask);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while deleting the task");
            }

            return Ok("Succesfully deleted!");
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
