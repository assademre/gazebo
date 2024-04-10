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

        public async Task<bool> IsEventMemberAlreadyAdded(int eventId, int userId)
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


        public async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
