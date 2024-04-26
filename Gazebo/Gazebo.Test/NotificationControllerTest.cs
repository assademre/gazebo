using AutoMapper;
using Gazebo.Controller;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Gazebo.Test
{
    [TestFixture]
    public class NotificationControllerTest
    {
        private NotificationController _controller;
        private Mock<INotificationRepository> _notificationRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private const int TestUserId = 1;

        [SetUp]
        public void Setup()
        {
            _notificationRepositoryMock = new Mock<INotificationRepository>();

            _controller = new NotificationController(_notificationRepositoryMock.Object);
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
        public async Task GetNotification_ReturnsOkResult()
        {
            // Arrange
            var notifications = GetTestNotifications();


            _notificationRepositoryMock.Setup(repo => repo.GetTaskNotifications(It.IsAny<int>()))
                           .ReturnsAsync(notifications);

            // Act
            var result = await _controller.GetNotification();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            var resultNotifications = okResult.Value as List<Notification>;
            Assert.That(resultNotifications?.Count, Is.EqualTo(notifications.Count));
        }

        [Test]
        public async Task MakeNotificationRead_ReturnsOkResult()
        {
            // Arrange
            const int notificationId = 1;

            _notificationRepositoryMock.Setup(repo => repo.MakeNotificationRead(It.IsAny<int>(), It.IsAny<int>()))
                                       .ReturnsAsync(true);

            // Act
            var result = await _controller.MakeNotificationRead(notificationId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.That(okResult.Value, Is.EqualTo(true));
        }

        private List<Notification> GetTestNotifications()
        {
            return new List<Notification>
            {
                new Notification { UserId = 1, Subject = "Subject 1", Body = "Body 1", CreatedDate = DateTime.UtcNow, IsRead = false },
                new Notification { UserId = 1, Subject = "Subject 2", Body = "Body 2", CreatedDate = DateTime.UtcNow, IsRead = false },
                new Notification { UserId = 1, Subject = "Subject 3", Body = "Body 3", CreatedDate = DateTime.UtcNow, IsRead = false }
            };
        }
    }
}
