using EventOrganizationApp.Models;

namespace EventOrganizationApp.Interfaces.Users
{
    public interface IProfileRepository
    {
        User GetUserInfo(int userId);
    }
}
