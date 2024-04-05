
using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Gazebo.Models;
using Gazebo.Security;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class UserAccessRepository : IUserAccessRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IHashPassword _hashPassword;
        public UserAccessRepository(DataContext context, IHashPassword hashPassword, IUserRepository userRepository)
        {
            _context = context;
            _hashPassword = hashPassword;
            _userRepository = userRepository;
        }

        public async Task<int> UserLogin(string username, string password)
        {
           if (username == null || password == null)
            {
                return 0;
            }

            var hashedPassword = _hashPassword.HashPassword(password);

            var respond = _context.UserAccess
                .Where(x => x.Username == username && x.PasswordHash == hashedPassword)
                .FirstOrDefault();

            return respond?.UserId ?? 0;
        }

        public async Task<bool> UserSignUp(string username, string password, string name, string surname, string email, string phoneNumber)
        {
            if (await IsUsernameNotExist(username))
            {
                return false;
            }

            var hashedPassword = _hashPassword.HashPassword(password);

            var newUser = new UserAccess
            {
                Username = username,
                PasswordHash = hashedPassword
            };

            if (newUser == null)
            {
                return false;
            }

            await _context.AddAsync(newUser);

            if (!await SaveChanges())
            {
                return false;
            }

            var addedUser = _context.UserAccess
                .Where(x =>  x.Username == username)
                .FirstOrDefault();

            if (addedUser == null)
            {
                return false;
            }

            var newUserInfo = new User
            {
                UserId = addedUser.UserId,
                Username = addedUser.Username,
                Name = name,
                Surname = surname,
                PhoneNumber = phoneNumber,
                Email = email
            };

            return _userRepository.CreateUser(newUserInfo);
        }

        internal async Task<bool> IsUsernameNotExist(string username)
        {
            if (username == string.Empty)
            {
                return false;
            }

            var response = await _context.UserAccess
                .Where(x => x.Username == username)
                .ToListAsync();

            return response.Count > 0;
        }

        private async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
