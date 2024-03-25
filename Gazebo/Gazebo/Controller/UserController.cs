using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using EventOrganizationApp.Repository.Users;
using Gazebo.Data.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizationApp.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _profileRepository;

        public IMapper _mapper;

        public UserController(IUserRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}/profile")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetProfileInfo(int userId)
        {
            var profileInfo = _profileRepository.GetUserInfo(userId);

            if (profileInfo.UserId == 0)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(profileInfo);
        }


        [HttpGet("user-list")]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUsersName()
        {
            var users = _mapper.Map<IList<UserDto>>(_profileRepository.GetUsersName());

            if (users.Count() == 0)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] User user)
        {
            var mappedUser = _mapper.Map<User>(user);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_profileRepository.CreateUser(mappedUser))
            {
                ModelState.AddModelError("", "Encounter an error while creating the user");
            }

            return Ok("Succesfully created!");
        }
    }
}
