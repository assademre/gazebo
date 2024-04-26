using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EventOrganizationApp.Controller;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using EventOrganizationApp.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Gazebo.Test
{
    [TestFixture]
    public class EventControllerTests
    {
        private EventController _controller;
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<IEventMemberRepository> _eventMemberRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private const int TestUserId = 1;

        [SetUp]
        public void Setup()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _eventMemberRepositoryMock = new Mock<IEventMemberRepository>();
            _mapperMock = new Mock<IMapper>();

            _eventRepositoryMock.Setup(repo => repo.GetEventIdByEventNameAndUserId(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(1);
            _eventMemberRepositoryMock.Setup(repo => repo.AddEventMember(It.IsAny<EventMember>())).ReturnsAsync(true);

            _controller = new EventController(_eventRepositoryMock.Object,
                _eventMemberRepositoryMock.Object,
                _mapperMock.Object);

            SetupHttpContext();
        }

        private void SetupHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", TestUserId.ToString())
            }));

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;
        }

        [Test]
        public async Task GetEventUserCreated_ReturnsOkResult()
        {
            // Arrange
            var events = GetTestEvents();
            var mappedEvents = MapEventsToDto(events);

            _eventRepositoryMock.Setup(repo => repo.GetEventsUserCreated(TestUserId)).ReturnsAsync(events);
            _mapperMock.Setup(m => m.Map<IList<EventDto>>(events)).Returns(mappedEvents);

            // Act
            var result = await _controller.GetEventUserCreated();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateEvent_ReturnsOkResult()
        {
            // Arrange
            var newEvent = GetTestEventDto();
            var mappedEvent = MapEventDtoToEvent(newEvent);

            _mapperMock.Setup(m => m.Map<Event>(newEvent)).Returns(mappedEvent);
            _eventRepositoryMock.Setup(repo => repo.CreateEvent(mappedEvent)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateEvent(newEvent);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private List<Event> GetTestEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    CreaterId = TestUserId,
                    EventName = "Event Test",
                    EventTypeId = (int)EventType.Default,
                    Budget = 100,
                    CurrencyId = (int)CurrencyModel.GBP,
                    Place = "Place Test",
                    StatusId = (int)Status.NotStarted,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    EventDate = DateTime.UtcNow
                }
            };
        }

        private List<EventDto> MapEventsToDto(List<Event> events)
        {
            return events.Select(e => _mapperMock.Object.Map<EventDto>(e)).ToList();
        }

        private EventDto GetTestEventDto()
        {
            return new EventDto
            {
                CreaterId = TestUserId,
                EventName = "Test Event",
                EventType = EventType.Default.ToString(),
                Budget = 200,
                Currency = CurrencyModel.EUR.ToString(),
                Place = "Testland",
                Status = Status.InProgress.ToString(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                EventDate = DateTime.UtcNow
            };
        }

        private Event MapEventDtoToEvent(EventDto eventDto)
        {
            return _mapperMock.Object.Map<Event>(eventDto);
        }
    }
}
