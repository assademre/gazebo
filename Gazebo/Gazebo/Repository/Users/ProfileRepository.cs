using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;

namespace EventOrganizationApp.Repository.Users
{
    public class ProfileRepository : IUserRepository
    {
        private readonly DataContext _context;

        public ProfileRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            throw new NotImplementedException();
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

        public IList<User> GetUsersName()
        {
            throw new NotImplementedException();
        }
    }
}
