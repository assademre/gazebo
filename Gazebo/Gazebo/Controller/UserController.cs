using AutoMapper;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EventOrganizationApp.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public IMapper _mapper;

        public UserController(IProfileRepository profileRepository, IMapper mapper)
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
    }
}
