using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using EventOrganizationApp.Repository.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet("eventId={eventId}/event-status")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusByEventId(int eventId)
        {
            var eventStatus = _eventRepository.GetStatusByEventId(eventId);

            if (eventStatus == string.Empty)
            {
                return NotFound();
            }

            return Ok(eventStatus);
        }
    }
}
