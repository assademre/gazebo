using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IUserRepository
    {
        User GetUserInfo(int userId);

        IList<User> GetUsersName();

        bool CreateUser(User user);
    }
}
