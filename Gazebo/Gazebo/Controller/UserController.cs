﻿using AutoMapper;
using EventOrganizationApp.Models;
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
            var userId = GetUser();

            if (userId == 0)
            {
                return BadRequest("The user not found");
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


        [HttpGet("userlist")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUsersName()
        {
            var userProfile = _profileRepository.GetUsersName();
            var users = _mapper.Map<IList<UserDto>>(userProfile);   

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
