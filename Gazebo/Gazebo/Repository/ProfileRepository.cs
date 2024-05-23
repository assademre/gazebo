using EventOrganizationApp.Data;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;

        public ProfileRepository(DataContext context, IUserRepository userRepository, IFriendshipRepository friendshipRepository)
        {
            _context = context;
            _userRepository = userRepository;   
            _friendshipRepository = friendshipRepository;
        }
        public async Task<ProfileDto> GetProfile(int userId, int profileId)
        {
            if (profileId == 0 || userId == 0)
            {
                return new ProfileDto();
            }

            var additionalData = await _context.AdditionalData
                .Where(p => p.UserId == profileId)
                .FirstOrDefaultAsync();

            var friendshipStatus = await _friendshipRepository.GetFriendshipStatus(userId, profileId);

            var userInfo = _userRepository.GetUserInfo(profileId);

            var profile = new ProfileDto
            {
                UserId = userId,
                Username = userInfo.Username,
                Name = userInfo.Name,
                Surname = userInfo.Surname,
                Email = userInfo.Email,
                PhoneNumber = additionalData.PhoneNumber,
                DateOfBirth = additionalData.DateOfBirth,
                FriendshipStatus = friendshipStatus,
                Bio = additionalData.Bio
            };

            return profile;
        }

        public async Task<bool> CreateProfile(Additional additionalData)
        {
            if (additionalData == null)
            {
                return false;
            }

            _context.Add(additionalData);

            return await SaveChanges();
        }

        public async Task<bool> UpdateProfile(int userId,Additional additionalData)
        {
            if (additionalData == null)
            {
                return false;
            }

            additionalData.UserId = userId;

            _context.Update(additionalData);

            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
