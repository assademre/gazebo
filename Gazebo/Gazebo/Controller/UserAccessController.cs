using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/user-access")]
    [ApiController]
    public class UserAccessController : ControllerBase
    {
        private readonly IUserAccessRepository _userAccessRepository;

        public UserAccessController(IUserAccessRepository userAccessRepository)
        {
            _userAccessRepository = userAccessRepository;
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

            if (!await _userAccessRepository.UserLogin(login.Username, login.Password))
            {
                ModelState.AddModelError("", "Wrong username or password");
            }

            return Ok("Login succesfull!");
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
