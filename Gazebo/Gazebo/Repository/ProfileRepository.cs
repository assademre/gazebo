using EventOrganizationApp.Data;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly DataContext _context;

        public ProfileRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Additional> GetProfile(int userId)
        {
            if (userId == 0)
            {
                return new Additional();
            }

            var profile = await _context.AdditionalData
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync();

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

        public async Task<bool> UpdateProfile(Additional additionalData)
        {
            if (additionalData == null)
            {
                return false;
            }

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
