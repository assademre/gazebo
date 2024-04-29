using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Repository;
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
        private readonly IEventMemberRepository _eventMemberRepository;
        public IMapper _mapper;

        public EventController(IEventRepository eventRepository, IEventMemberRepository eventMemberRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _eventMemberRepository = eventMemberRepository;
        }

        [HttpGet("createdevents")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEventUserCreated()
        {
            var userId = GetUser();

            if (userId == 0) 
            {
                return BadRequest("The user not found");
            }

            var createdEvents = await _eventRepository.GetEventsUserCreated(userId);
            var mappedEvents = _mapper.Map<IList<EventDto>>(createdEvents);

            if (mappedEvents.IsNullOrEmpty())
            {
                return BadRequest("events not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(mappedEvents);
        }

        [HttpGet("myevents")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IList<Event>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMyEvents()
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var userMemberEvents = await _eventMemberRepository.GetUserEvents(userId);

            var createdEvents = await _eventRepository.GetEventsByEventsId(userMemberEvents);

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
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var isMember = await _eventMemberRepository.IsUserMember(eventId, userId);

            if (isMember == false)
            {
                return BadRequest("User does not have the permission for this action");
            }

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

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto newEvent)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }
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

            var eventId = await _eventRepository.GetEventIdByEventNameAndUserId(userId, newEvent.EventName);

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
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            var isAdmin = await _eventMemberRepository.IsUserAdmin(eventDto.EventId, userId);

            if (!isAdmin)
            {
                return BadRequest("The user has no permission for this action");
            }

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

        [HttpPut("{eventId}/{ownerId}/admin")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(User))]
        public async Task<IActionResult> SetUserAsAdminAsync([FromRoute] int eventId, [FromRoute] int ownerId)
        {
            var isOwnerAdmin = await _eventMemberRepository.IsUserAdmin(eventId, ownerId);

            if (isOwnerAdmin)
            {
                return Ok(isOwnerAdmin);
            }

            var userId = GetUser();

            var isUserAdmin = await _eventMemberRepository.IsUserAdmin(eventId, userId);

            if (!isUserAdmin)
            {
                return BadRequest("User does not have a permission for this action.");
            }

            var response = await _eventMemberRepository.SetUserAsAdmin(eventId, ownerId);

            return Ok(response);
        }

        [HttpDelete("{eventId:int}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteEvent([FromRoute] int eventId)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
            }

            if (eventId == 0)
            {
                ModelState.AddModelError("", "eventId is wrong");
            }

            var eventInfo = await _eventRepository.GetEventByEventId(eventId);

            if (userId != eventInfo.CreaterId)
            {
                return BadRequest("User does not have a permission for this action.");
            }

            var mappedEvent = _mapper.Map<Event>(eventInfo);

            var result = await _eventRepository.DeleteEvent(mappedEvent);
            if (!result)
            {
                ModelState.AddModelError("", "Encounter an error while deleting the event");
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
