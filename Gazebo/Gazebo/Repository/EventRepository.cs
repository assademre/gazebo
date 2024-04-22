using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Gazebo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IList<Event>> GetEventsUserCreated(int userId)
        {
            if (userId == 0)
            {
                return null;
            }

            var userEvents = await _context.Events
                .Where(x => x.CreaterId == userId)
                .ToListAsync();

            if (userEvents == null || userEvents.Count() == 0)
            {
                return null;
            }

            return userEvents;
        }

        public async Task<int> GetEventIdByEventNameAndUserId(int userId, string eventName)
        {
            if (userId == 0 || eventName == string.Empty)
            {
                return 0;
            }

            var eventId = await _context.Events
                .Where(x => x.CreaterId == userId && x.EventName == eventName)
                .Select(x => x.EventId)
                .FirstOrDefaultAsync();

            return eventId;
        }

        public async Task<bool> CreateEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                return false;
            }

            await _context.AddAsync(newEvent);

            return await SaveChanges();
        }

        public async Task<bool> UpdateEvent(Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                return false;
            }

            _context.Update(updatedEvent);

            return await SaveChanges();
        }

        public async Task<Event> GetEventByEventId(int eventId)
        {
            if (eventId == 0)
            {
                return null;
            }

            var result = await _context.Events
                .Where(x => x.EventId == eventId)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Event>> GetEventsByEventsId(List<int> eventIds)
        {
            if (!eventIds.Any())
            {
                return new List<Event>();
            }

            var result = await _context.Events
                .Where(x => eventIds.Contains(x.EventId))
                .ToListAsync();

            return result;
        }

        public async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
