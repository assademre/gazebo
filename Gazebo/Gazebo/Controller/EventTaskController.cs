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

        [HttpGet("{userId}/all-tasks")]
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

        [HttpGet("{userId}/event-task")]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
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
    }
}
