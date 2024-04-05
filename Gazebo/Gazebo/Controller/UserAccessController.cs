using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Gazebo.Models;
using Gazebo.Security;
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

            var token = _tokenGenerator.GenerateToken(login.Username);

            return Ok(new { Token = token, UserId = userId });
        }

        [HttpPost("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UserSignUp([FromBody] SignUp signUp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _userAccessRepository.UserSignUp(signUp.Username, signUp.Password, signUp.Name, signUp.Surname, signUp.Email, signUp.PhoneNumber))
            {
                ModelState.AddModelError("", "Encounter an error while creating the user");
            }

            return Ok("Succesfully created!");
        }
    }
}
