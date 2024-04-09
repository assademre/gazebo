using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Gazebo.Models;
using Gazebo.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/user-access")]
    [ApiController]
    public class UserAccessController : ControllerBase
    {
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public UserAccessController(IUserAccessRepository userAccessRepository, ITokenGenerator tokenGenerator)
        {
            _userAccessRepository = userAccessRepository;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet("username-availability/{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> IsUsernameTaken([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUsernameTaken = await _userAccessRepository.IsUsernameOrEmailExists(username);

            return Ok(new { Response = isUsernameTaken });
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UserLogin([FromBody] Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = await _userAccessRepository.UserLogin(login.Username, login.Password);

            if (userId == 0)
            {
                ModelState.AddModelError("", "Wrong username or password");
                return BadRequest(ModelState);
            }

            var token = _tokenGenerator.GenerateToken(userId.ToString());

            return Ok(new { Token = token, UserId = userId });
        }

        [HttpPost("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UserSignUp([FromBody] SignUp signUp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _userAccessRepository.UserSignUp(signUp))
            {
                return BadRequest("Encountered an error during signing up");
            }

            return Ok("Succesfully created!");
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout successful" });
        }
    }
}
