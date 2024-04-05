using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IUserAccessRepository
    {
        Task<bool> UserLogin(string username, string password);
        Task<bool> UserSignUp(string username, string password, string name, string surname, string email, string phoneNumber);
    }
}
