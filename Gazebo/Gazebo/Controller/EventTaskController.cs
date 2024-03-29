using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Gazebo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

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

        [HttpGet("{userId:int}/all-tasks")]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllUserTasks([FromRoute] int userId)
        {
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

        [HttpGet("userid={userId:int}&eventid={eventId:int}/event-task")]
        [ProducesResponseType(200, Type = typeof(IList<EventTaskDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserTasksForAnEvent([FromRoute] int userId, [FromRoute] int eventId)
        {
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
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTask([FromRoute] int taskId)
        {
            var taskInfo = await _eventTaskRepository.GetTask(taskId);
            var mappedTaskInfo = _mapper.Map<EventTaskDto>(taskInfo);

            if (mappedTaskInfo == null)
            {
                return NotFound();
            }

            return Ok(mappedTaskInfo);
        }

        [HttpPost]
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

            return Ok("Succesfully created!");
        }

        [HttpPut("{taskId:int}/status")]
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
