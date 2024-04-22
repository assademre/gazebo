using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Gazebo.Repository
{
    public class EventMemberRepository : IEventMemberRepository
    {
        private readonly DataContext _context;
        public EventMemberRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEventMember(EventMember eventMember)
        {
            if (eventMember == null)
            {
                return false;
            }

            _context.Add(eventMember);

            return await SaveChanges();
        }

        public async Task<bool> IsUserMember(int eventId, int userId)
        {
            if (eventId == 0 || userId == 0)
            {
                return false;
            }

            var member = await _context.EventMembers
                .Where(x => x.UserId == userId && x.EventId == eventId)
                .ToListAsync();


            return member.Count() > 0;
        }

        public async Task<bool> IsUserAdmin(int eventId, int userId)
        {
            if (eventId == 0 || userId == 0)
            {
                return false;
            }

            var isAdmin = await _context.EventMembers
                .AnyAsync(x => x.UserId == userId && x.EventId == eventId && x.IsAdmin);

            return isAdmin;
        }

        public async Task<List<int>> GetAdminEvents(int userId)
        {
            if (userId == 0)
            {
                return new List<int>();
            }

            var adminEvents = await _context.EventMembers
                .Where(x => x.IsAdmin && x.UserId == userId)
                .Select(x => x.EventId)
                .ToListAsync();

            return adminEvents;
        }

        public async Task<List<int>> GetUserEvents(int userId)
        {
            if (userId == 0)
            {
                return new List<int>();
            }

            var userEvents = await _context.EventMembers
                .Where(x => x.UserId == userId)
                .Select(x => x.EventId)
                .ToListAsync();

            return userEvents;
        }


        public async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
