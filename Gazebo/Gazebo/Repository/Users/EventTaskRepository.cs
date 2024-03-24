using EventOrganizationApp.Data;
using EventOrganizationApp.Interfaces.Users;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;

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

        public IList<EventsTask> GetTasksForEvent(int eventId)
        {
            if (eventId == 0)
            {
                throw new ArgumentException($"eventId {eventId} is not correct");
            }

            var tasksForAnEvent = _context.Tasks
                .Where (x => x.EventId == eventId)
                .ToList();

            if (tasksForAnEvent == null || tasksForAnEvent.Count() == 0)
            {
                throw new ArgumentException("No event has been found");
            }

            return tasksForAnEvent;
        }

        public string GetStatusByTaskId(int taskId)
        {
            if (taskId == 0)
            {
                return string.Empty;
            }

            var taskStatus = _context.Tasks
                .Where(x => x.TaskId == taskId)
                .Select(x => ((Status)x.StatusId).ToString())
                .FirstOrDefault() ?? string.Empty;

            return taskStatus;
        }
    }
}
