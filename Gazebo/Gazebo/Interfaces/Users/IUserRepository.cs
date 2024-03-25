using EventOrganizationApp.Models;

namespace EventOrganizationApp.Interfaces.Users
{
    public interface IUserRepository
    {
        User GetUserInfo(int userId);

        IList<User> GetUsersName();

        bool CreateUser(User user);
    }
}
