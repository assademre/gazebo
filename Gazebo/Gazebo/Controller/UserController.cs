using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Repository;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetProfileInfo()
        {
            var claim = User.Claims
                .FirstOrDefault(x => x.Type == "userId");

            if (claim == null)
            {
                return BadRequest("The userId claim is missing");
            }
            var userIdString = claim?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("The userId claim is not a valid integer");
            }

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
        [Authorize]
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
