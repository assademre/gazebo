using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Gazebo.Repository;
using Gazebo.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EventOrganizationApp.Controller
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventMemberRepository _eventMemberRepository;
        public IMapper _mapper;

        public EventController(IEventRepository eventRepository, IMapper mapper, IEventMemberRepository eventMemberRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _eventMemberRepository = eventMemberRepository;
        }

        [HttpGet("created-events")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEventUserCreated()
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

            var eventId = await _eventRepository.GetEventIdByEventNameAndUserId(newEvent.CreaterId, newEvent.EventName);

            if (eventId == 0)
            {
                return BadRequest("Encounter an error while creating the event");
            }

            var eventMember = new EventMember
            {
                EventId = eventId,
                UserId = newEvent.CreaterId,
                IsAdmin = true
            };

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
        public async Task<IActionResult> EditEvent([FromBody] EventDto eventDto)
        {
            var mappedEvent = _mapper.Map<Event>(eventDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventRepository.UpdateEvent(mappedEvent);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while updating the task");
            }

            return Ok("Succesfully updated!");
        }
    }
}
