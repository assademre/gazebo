using EventOrganizationApp.Data;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;

namespace EventOrganizationApp.Repository.Users
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly DataContext _context;

        public ProfileRepository(DataContext context)
        {
            _context = context;
        }

        public User GetUserInfo(int userId)
        {
            if (userId == 0)
            {
                return new User();
            }

            var userInfo = _context.Users
                .Where(x => x.UserId == userId)
                .FirstOrDefault() ?? new User();
            
            if (userInfo.UserId == 0)
            {
                return new User();
            }

            return userInfo;
        }
    }
}
