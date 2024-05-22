using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet("{profileId}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProfile(int profileId)
        {
            var userId = GetUser();

            if (profileId == 0 || userId == 0)
            {
                return BadRequest("user id or profileId is not correct");
            }

            var profile = await _profileRepository.GetProfile(userId, profileId);

            if (profile == null)
            {
                return BadRequest($"no profile found under this userId = {userId}");
            }

            return Ok(profile);
        }

        [HttpPut()]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProfile(Additional additionalData)
        {
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("user id is not correct");
            }

            if (additionalData == null) 
            {
                return BadRequest($"profile is not correct"); 
            }


            var updateResponse = await _profileRepository.UpdateProfile(additionalData);

            if (!updateResponse)
            {
                return BadRequest($"error while updating the profile");
            }

            return Ok(additionalData);
        }

        private int GetUser()
        {
            var claim = User.Claims
               .FirstOrDefault(x => x.Type == "userId");

            if (claim == null)
            {
                return 0;
            }
            var userIdString = claim?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return 0;
            }

            return userId;
        }
    }
}
