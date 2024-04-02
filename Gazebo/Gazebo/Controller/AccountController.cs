using Gazebo.Data.Dto;
using Gazebo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gazebo.Controller
{
    [Route("api/account")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.EmailAddress
                };

                var cretedUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if(cretedUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok("User Created");
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }

                else
                {
                    return BadRequest(cretedUser.Errors);
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
