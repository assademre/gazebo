using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Gazebo.Interfaces;
using Gazebo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EventOrganizationApp.Controller
{
    [Route("api/event-task")]
    [ApiController]
    public class EventTaskController : ControllerBase
    {
        private readonly IEventTaskRepository _eventTaskRepository;
        private readonly IEventMemberRepository _eventMemberRepository;

        public IMapper _mapper;

        public EventTaskController(IEventTaskRepository eventTaskRepository, IMapper mapper, IEventMemberRepository eventMemberRepository)
        {
            _eventTaskRepository = eventTaskRepository;
            _mapper = mapper;
            _eventMemberRepository = eventMemberRepository;
        }

        
        [HttpGet("all-tasks")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllUserTasks()
        {
            var claim = User.Claims
                .FirstOrDefault(x => x.Type == "userId");

            if (claim == null)
            {
                return BadRequest("The userId claim is missing");
            }
            var userIdString = claim?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("The userId claim is not a valid integer");
            }

            var allTasks = await _eventTaskRepository.GetAllUserTasks(userId);
            var mappedTasks = _mapper.Map<IList<EventTaskDto>>(allTasks);

            if (mappedTasks.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedTasks);
        }

        [HttpGet("eventid={eventId:int}/event-task")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<EventTaskDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserTasksForAnEvent([FromRoute] int eventId)
        {
            var claim = User.Claims
                .FirstOrDefault(x => x.Type == "userId");

            if (claim == null)
            {
                return BadRequest("The userId claim is missing");
            }
            var userIdString = claim?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("The userId claim is not a valid integer");
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
            var claim = User.Claims
               .FirstOrDefault(x => x.Type == "userId");

            if (claim == null)
            {
                return BadRequest("The userId claim is missing");
            }
            var userIdString = claim?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("The userId claim is not a valid integer");
            }

            var taskInfo = await _eventTaskRepository.GetTask(taskId);

            var isUserMember = await _eventMemberRepository.IsUserMember(taskInfo.EventId, userId);

            if (!isUserMember)
            {
                return BadRequest("User has no access to this task");
            }

            var mappedTaskInfo = _mapper.Map<EventTaskDto>(taskInfo);

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
            var mappedTask = _mapper.Map<EventsTask>(task);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventTaskRepository.CreateTask(mappedTask);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while creating the task");
            }

            var eventMember = new EventMember
            {
                EventId = task.EventId,
                UserId = task.OwnerId,
                IsAdmin = false
            };

            var isUserAdded = await _eventMemberRepository.IsUserMember(task.EventId, task.OwnerId);
            if (isUserAdded)
            {
                return Ok("Succesfully created!");
            }

            var response = await _eventMemberRepository.AddEventMember(eventMember);

            if (!response)
            {
                return BadRequest("Encounter an error while creating the event");
            }

            return Ok("Succesfully created!");
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> EditTask([FromBody] EventTaskDto task)
        {
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
            if (taskId == 0)
            {
                ModelState.AddModelError("", "taskId is wrong");
            }

            var task = await _eventTaskRepository.GetTask(taskId);
            var mappedTask = _mapper.Map<EventsTask>(task);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventTaskRepository.DeleteTask(mappedTask);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while deleting the task");
            }

            return Ok("Succesfully deleted!");
        }
    }
}
