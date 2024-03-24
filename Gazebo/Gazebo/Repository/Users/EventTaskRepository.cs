using EventOrganizationApp.Data;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;

namespace EventOrganizationApp.Repository.Users
{
    public class EventTaskRepository : IEventTaskRepository
    {
        private readonly DataContext _context;
        public EventTaskRepository(DataContext context)
        {
            _context = context;
        }

        public IList<EventsTask> GetAllUserTasks(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException("userId is 0");
            }

            var userTasks = _context.Tasks
                .Where(x => x.OwnerId == userId)
                .ToList();

            if (userTasks == null || userTasks.Count() == 0)
            {
                throw new ArgumentException("No event has been found");
            }

            return userTasks;
        }

        public IList<EventsTask> GetUserTasksForAnEvent(int userId, int eventId)
        {
            if (userId == 0 || eventId == 0)
            {
                throw new ArgumentException($"userId {userId} or eventId {eventId} is 0");
            }

            var userTaskForAnEvent = _context.Tasks
                .Where(x => x.EventId == eventId)
                .Where(x => x.OwnerId == userId)
                .ToList();

            if (userTaskForAnEvent == null || userTaskForAnEvent.Count() == 0)
            {
                throw new ArgumentException("No event has been found");
            }

            return userTaskForAnEvent;
        }
    }
}
