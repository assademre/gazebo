using EventOrganizationApp.Data;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;

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
    }
}
