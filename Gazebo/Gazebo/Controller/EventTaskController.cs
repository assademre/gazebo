using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EventOrganizationApp.Controller
{
    [Route("api/event-task")]
    [ApiController]
    public class EventTaskController : ControllerBase
    {
        private readonly IEventTaskRepository _eventTaskRepository;

        public IMapper _mapper;

        public EventTaskController(IEventTaskRepository eventTaskRepository, IMapper mapper)
        {
            _eventTaskRepository = eventTaskRepository;
            _mapper = mapper;
        }

        [HttpGet("userid={userId}/all-tasks")]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllUserTasks(int userId)
        {
            var allTasks = _mapper.Map<IList<EventTaskDto>>(_eventTaskRepository.GetAllUserTasks(userId));

            if (allTasks.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(allTasks);
        }

        [HttpGet("userid={userId}&eventid={eventId}/event-task")]
        [ProducesResponseType(200, Type = typeof(IList<EventTaskDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserTasksForAnEvent(int userId, int eventId)
        {
            var userEventTask = _mapper.Map<IList<EventTaskDto>>(_eventTaskRepository.GetUserTasksForAnEvent(userId, eventId));

            if (userEventTask.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(userEventTask);
        }

        [HttpGet("eventid={eventId}/tasks")]
        [ProducesResponseType(200, Type = typeof(IList<EventTaskDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTasksForEvent(int eventId)
        {
            var tasksList = _mapper.Map<IList<EventTaskDto>>(_eventTaskRepository.GetTasksForEvent(eventId));

            if (tasksList.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tasksList);
        }

        [HttpGet("taskid={taskId}/task-status")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusByTaskId(int taskId)
        {
            var taskStatus = _eventTaskRepository.GetStatusByTaskId(taskId);

            if (taskStatus == string.Empty)
            {
                return NotFound();
            }

            return Ok(taskStatus);
        }
    }
}
