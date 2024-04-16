using System.ComponentModel.DataAnnotations;

namespace Gazebo.Data.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Username must be minimum 5 characters")]
        [MaxLength(20, ErrorMessage = "Username cannot access 20 characters")]
        public string Username { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot access 50 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Surname cannot access 50 characters")]
        public string Surname { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Email cannot access 50 characters")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Phone number cannot access 50 characters")]
        public string PhoneNumber { get; set; }
    }
}
