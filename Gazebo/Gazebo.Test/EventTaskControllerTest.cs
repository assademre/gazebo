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
    public class EventTaskControllerTest
    {
        private EventTaskController _controller;
        private Mock<IEventTaskRepository> _eventTaskRepositoryMock;
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<INotificationRepository> _notificationRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IEventMemberRepository> _eventMemberRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private const int TestUserId = 1;
        private const int TestOwnerId = 1;
        private const int TestEventId = 1;

        [SetUp]
        public void Setup()
        {
            _eventTaskRepositoryMock = new Mock<IEventTaskRepository>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _eventMemberRepositoryMock = new Mock<IEventMemberRepository>();
            _mapperMock = new Mock<IMapper>();

            _eventTaskRepositoryMock.Setup(repo => repo.GetAllUserTasks(It.IsAny<int>())).ReturnsAsync(new List<EventsTask>());
            _eventTaskRepositoryMock.Setup(repo => repo.CreateTask(It.IsAny<EventsTask>())).ReturnsAsync(true);
            _eventTaskRepositoryMock.Setup(repo => repo.GetAllUserTasks(It.IsAny<int>()))
                            .ReturnsAsync(new List<EventsTask> { new EventsTask() }); // TODO: send EventTask

            _eventRepositoryMock.Setup(repo => repo.GetEventByEventId(It.IsAny<int>())).ReturnsAsync(new Event());

            _eventMemberRepositoryMock.Setup(repo => repo.AddEventMember(It.IsAny<EventMember>())).ReturnsAsync(true);
            _eventMemberRepositoryMock.Setup(repo => repo.IsUserAdmin(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _eventMemberRepositoryMock.Setup(repo => repo.IsUserMember(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            _userRepositoryMock.Setup(repo => repo.GetUserInfo(It.IsAny<int>()))
                   .Returns(new User { UserId = TestUserId, Username = "Test User" });

            _notificationRepositoryMock.Setup(repo => repo.CreateNewTaskNotification(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<IList<EventTaskDto>>(It.IsAny<IEnumerable<EventsTask>>()))
                        .Returns((IEnumerable<EventsTask> tasks) =>
                        {
                            return tasks.Select(t => new EventTaskDto()).ToList();
                        });

            _controller = new EventTaskController(_eventTaskRepositoryMock.Object,
                _eventRepositoryMock.Object, 
                _eventMemberRepositoryMock.Object, 
                _notificationRepositoryMock.Object, 
                _userRepositoryMock.Object,
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
        public async Task GetAllUserTasks_ReturnsOkResult()
        {
            // Arrange
            var tasks = GetTestTasks();
            var mappedTasks = MapTasksToDto(tasks);

            _mapperMock.Setup(m => m.Map<IList<EventTaskDto>>(tasks)).Returns(mappedTasks);

            // Act
            var result = await _controller.GetAllUserTasks();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateTask_ReturnsOkResult()
        {
            // Arrange
            var newTask = GetTestTaskDto();
            var mappedTask = MapTaskDtoToTask(newTask);

            _mapperMock.Setup(m => m.Map<EventsTask>(newTask)).Returns(mappedTask);
            _eventTaskRepositoryMock.Setup(repo => repo.CreateTask(mappedTask)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateTask(newTask);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private List<EventsTask> GetTestTasks()
        {
            return new List<EventsTask>
            {
                new EventsTask
                {
                    EventId = TestEventId,
                    TaskName = "Task Test",
                    Budget = 100,
                    CurrencyId = (int)CurrencyModel.GBP,
                    Place = "Place Test",
                    StatusId = (int)Status.NotStarted,
                    OwnerId = TestOwnerId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    TaskDate = DateTime.UtcNow
                }
            };
        }

        private List<EventTaskDto> MapTasksToDto(List<EventsTask> tasks)
        {
            return tasks.Select(e => _mapperMock.Object.Map<EventTaskDto>(e)).ToList();
        }

        private EventTaskDto GetTestTaskDto()
        {
            return new EventTaskDto
            {
                EventId = TestEventId,
                EventName = "Event Test",
                TaskName = "Task Test",
                Budget = 100,
                Currency = CurrencyModel.EUR.ToString(),
                Place = "Place Test",
                Status = Status.InProgress.ToString(),
                OwnerId = TestOwnerId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                TaskDate = DateTime.UtcNow
            };
        }

        private EventsTask MapTaskDtoToTask(EventTaskDto eventTaskDto)
        {
            return _mapperMock.Object.Map<EventsTask>(eventTaskDto);
        }
    }
}
