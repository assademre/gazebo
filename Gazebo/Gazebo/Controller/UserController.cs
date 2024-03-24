using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using Gazebo.Data.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
    }
}
