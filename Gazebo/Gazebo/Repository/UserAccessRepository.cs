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
        private readonly IProfileRepository _profileRepository;
        public UserAccessRepository(DataContext context, IHashPassword hashPassword, IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _context = context;
            _hashPassword = hashPassword;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<int> UserLogin(string username, string password)
        {
           if (username == null || password == null)
            {
                return 0;
            }

            var hashedPassword = _hashPassword.HashPassword(password);

            var respond = await _context.UserAccess
                .Where(x => x.Username == username && x.PasswordHash == hashedPassword)
                .FirstOrDefaultAsync();

            return respond?.UserId ?? 0;
        }

        public async Task<bool> UserSignUp(SignUp signup)
        {
            if (await IsUsernameOrEmailExists(signup.Username, signup.Email))
            {
                return false;
            }

            var hashedPassword = _hashPassword.HashPassword(signup.Password);

            var newUser = new UserAccess
            {
                Username = signup.Username.ToLower(),
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
                .Where(x =>  x.Username == signup.Username)
                .FirstOrDefault();

            if (addedUser == null)
            {
                return false;
            }

            var newUserInfo = new User
            {
                UserId = addedUser.UserId,
                Username = addedUser.Username,
                Name = signup.Name,
                Surname = signup.Surname,
                Email = signup.Email
            };

            var userCreation = _userRepository.CreateUser(newUserInfo);

            if (!userCreation)
            {
                return false;
            }

            var additionalData = new Additional
            {
                UserId = addedUser.UserId,
                PhoneNumber = "test",
                DateOfBirth = DateTime.UtcNow,
                Bio = "test"
            };

            var profileCreate = await _profileRepository.CreateProfile(additionalData);

            if (!profileCreate)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsUsernameOrEmailExists(string username)
        {
            if (username == string.Empty)
            {
                return false;
            }

            var response = await _context.Users
                .Where(x => x.Username == username)
                .ToListAsync();

            return response.Count > 0;
        }

        internal async Task<bool> IsUsernameOrEmailExists(string username, string email)
        {
            if (username == string.Empty || email == string.Empty)
            {
                return false;
            }

            var response = await _context.Users
                .Where(x => x.Username == username || x.Email == email)
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
