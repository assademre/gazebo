using Gazebo.Data.Dto;
using Gazebo.Models;

namespace Gazebo.Interfaces
{
    public interface IProfileRepository
    {
        Task<ProfileDto> GetProfile(int userId);
        Task<bool> CreateProfile(Additional additionalData);
        Task<bool> UpdateProfile(Additional additionalData);
    }
}
