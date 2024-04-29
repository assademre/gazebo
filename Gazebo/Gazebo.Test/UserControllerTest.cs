using AutoMapper;
using EventOrganizationApp.Controller;
using EventOrganizationApp.Models;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using EventOrganizationApp.Data.Dto;

namespace Gazebo.Test
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserRepository> _mockProfileRepository;
        private Mock<IMapper> _mapperMock;
        private const string TestUsername = "test username";
        private const string TestEmail = "test@test.com";
        private const string TestPhoneNumber = "0123456789";
        private const int TestUserId = 1;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, User>());
            _mapperMock = new Mock<IMapper>();

            _mockProfileRepository = new Mock<IUserRepository>();
            _mapperMock.Setup(m => m.Map<IList<UserDto>>(It.IsAny<IEnumerable<User>>()))
                        .Returns((IEnumerable<User> tasks) =>
                        {
                            return tasks.Select(t => new UserDto()).ToList();
                        });
            _controller = new UserController(_mockProfileRepository.Object, _mapperMock.Object);
        }

        [Test]
        public void GetProfileInfo_UserFound_ReturnsOk()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new System.Security.Claims.Claim("userId", TestUserId.ToString())
                    }))
                }
            };

            var profileInfo = new User { UserId = TestUserId };
            _mockProfileRepository.Setup(repo => repo.GetUserInfo(TestUserId)).Returns(profileInfo);

            // Act
            var result = _controller.GetProfileInfo();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.Value, Is.EqualTo(profileInfo));
        }

        [Test]
            public void GetUsersName_UsersFound_ReturnsOk()
            {
                // Arrange
                var users = new List<User> {
                    new User 
                    {
                        UserId = TestUserId, 
                        Username = TestUsername,
                        Name = TestUsername,
                        Surname = TestUsername,
                        Email = TestEmail,
                        PhoneNumber = TestPhoneNumber

                    } 
                };
                _mockProfileRepository.Setup(repo => repo.GetUsersName()).Returns(users);

                // Act
                var result = _controller.GetUsersName();

                // Assert
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                var okResult = result as OkObjectResult;
                Assert.That(okResult, Is.Not.Null);
            }
    }
}
