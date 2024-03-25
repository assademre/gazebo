using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Repository;
using Gazebo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace EventOrganizationApp.Controller
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public IMapper _mapper;

        public EventController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}/created-events")]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public IActionResult GetEventUserCreated(int userId)
        {
            var createdEvents = _mapper.Map<IList<EventDto>>(_eventRepository.GetEventsUserCreated(userId));

            if (createdEvents.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(createdEvents);
        }

        [HttpGet("{eventId}/event-status")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusByEventId(int eventId)
        {
            var eventStatus = _eventRepository.GetStatusByEventId(eventId);

            if (eventStatus == string.Empty)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(eventStatus);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateEvent([FromBody] EventDto newEvent)
        {
            var mappedEvent = _mapper.Map<Event>(newEvent);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_eventRepository.CreateEvent(mappedEvent))
            {
                ModelState.AddModelError("", "Encounter an error while creating the event");
            }

            return Ok("Succesfully created!");
        }
    }
}
