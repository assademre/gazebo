using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Gazebo.Security;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{userId:int}/created-events")]
        [Authorize(Policy = "UserIdRequired")]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEventUserCreated([FromRoute] int userId)
        {
            var createdEvents = await _eventRepository.GetEventsUserCreated(userId);
            var mappedEvents = _mapper.Map<IList<EventDto>>(createdEvents);

            if (mappedEvents.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedEvents);
        }

        [HttpGet("{eventId:int}/event")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEventByEventId([FromRoute] int eventId)
        {
            var wholeEvent = await _eventRepository.GetEventByEventId(eventId);

            var mappedEvents = _mapper.Map<EventDto>(wholeEvent);

            if (mappedEvents == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedEvents);
        }


        [HttpGet("{eventId:int}/event-status")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetStatusByEventId([FromRoute] int eventId)
        {
            var eventStatus = await _eventRepository.GetStatusByEventId(eventId);

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
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto newEvent)
        {
            var mappedEvent = _mapper.Map<Event>(newEvent);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _eventRepository.CreateEvent(mappedEvent);

            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while creating the event");
            }

            return Ok("Succesfully created!");
        }
    }
}
