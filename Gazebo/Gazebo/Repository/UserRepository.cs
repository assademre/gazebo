using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;

namespace Gazebo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
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

        public IList<User> GetUsersName()
        {
            var users = _context.Users.ToList();

            if (users.Count() == 0)
            {
                return new List<User>();
            }

            return users;
        }

        public bool CreateUser(User user)
        {
            if (user == null)
            {
                return false;
            }

            _context.Add(user);

            return SaveChanges();
        }

        internal bool SaveChanges()
        {
            var savedData = _context.SaveChanges();

            return savedData > 0;
        }
    }
}
