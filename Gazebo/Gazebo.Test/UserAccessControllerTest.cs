using Gazebo.Controller;
using Gazebo.Interfaces;
using Gazebo.Models;
using Gazebo.Security;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gazebo.Test
{
    [TestFixture]
    public class UserAccessControllerTests
    {
        private UserAccessController _controller;
        private Mock<IUserAccessRepository> _mockUserAccessRepository;
        private Mock<ITokenGenerator> _mockTokenGenerator;
        private const string TestUsername = "test username";
        private const string TestPassword = "password";
        private const string TestEmail = "test@test.com";
        private const int TestUserId = 1;
        private const string TestToken = "test token";

        [SetUp]
        public void Setup()
        {
            _mockUserAccessRepository = new Mock<IUserAccessRepository>();
            _mockTokenGenerator = new Mock<ITokenGenerator>();
            _controller = new UserAccessController(_mockUserAccessRepository.Object, _mockTokenGenerator.Object);
        }

        [Test]
        public async Task IsUsernameTaken_ValidUsername_ReturnsOk()
        {
            // Arrange
            _mockUserAccessRepository.Setup(repo => repo.IsUsernameOrEmailExists(TestUsername.ToLower())).ReturnsAsync(false);

            // Act
            var result = await _controller.IsUsernameTaken(TestUsername);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UserLogin_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var login = new Login { Username = TestUsername, Password = TestPassword };
            _mockUserAccessRepository.Setup(repo => repo.UserLogin(login.Username, login.Password)).ReturnsAsync(TestUserId);
            _mockTokenGenerator.Setup(tokenGen => tokenGen.GenerateToken(TestUserId.ToString())).Returns(TestToken);

            // Act
            var result = await _controller.UserLogin(login);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.IsNotNull(okResult.Value);
        }

        [Test]
        public async Task UserSignUp_ValidSignUpData_ReturnsOk()
        {
            // Arrange
            var signUp = new SignUp { Username = TestUsername, Password = TestPassword, Email = TestEmail };
            _mockUserAccessRepository.Setup(repo => repo.UserSignUp(signUp)).ReturnsAsync(true);

            // Act
            var result = await _controller.UserSignUp(signUp);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Logout_Authorized_ReturnsOk()
        {
            // Act
            var result = _controller.Logout();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
