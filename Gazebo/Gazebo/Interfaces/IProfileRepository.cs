using Gazebo.Models;

namespace Gazebo.Interfaces
{
    public interface IProfileRepository
    {
        Task<Additional> GetProfile(int userId);
        Task<bool> CreateProfile(Additional additionalData);
        Task<bool> UpdateProfile(Additional additionalData);
    }
}
