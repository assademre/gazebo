using EventOrganizationApp.Models;
using Gazebo.Models;

namespace Gazebo.Interfaces
{
    public interface IUserAccessRepository
    {
        Task<int> UserLogin(string username, string password);
        Task<bool> UserSignUp(SignUp signUp);
    }
}
