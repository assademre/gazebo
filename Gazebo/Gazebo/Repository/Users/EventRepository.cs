using EventOrganizationApp.Data;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;

namespace EventOrganizationApp.Repository.Users
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public IList<Event> GetEventsUserCreated(int userId)
        {
            if (userId == 0)
            {
                return null;
            }

            var userEvents = _context.Events
                .Where(x => x.CreaterId == userId)
                .ToList();

            if (userEvents == null || userEvents.Count() == 0)
            {
                return null;
            }

            return userEvents;
        }

        public string GetStatusByEventId(int eventId)
        {
            if (eventId == 0)
            {
                return string.Empty;
            }

            var eventStatus = _context.Events
                .Where(x => x.EventId == eventId)
                .Select(x => ((Status)x.StatusId).ToString())
                .FirstOrDefault() ?? string.Empty;

            return eventStatus;
        }

        public bool CreateEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                return false;
            }

            _context.Add(newEvent);   
            
            return SaveChanges();
        }

        public bool SaveChanges()
        {
            var savedData = _context.SaveChanges();

            return savedData > 0;
        }
    }
}
