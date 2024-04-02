using System.ComponentModel.DataAnnotations;

namespace Gazebo.Data.Dto
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string? Password { get; set; }
    }
}
