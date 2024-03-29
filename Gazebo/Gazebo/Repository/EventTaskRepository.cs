using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using Gazebo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class EventTaskRepository : IEventTaskRepository
    {
        private readonly DataContext _context;
        public EventTaskRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IList<EventsTask>> GetAllUserTasks(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException("userId is 0");
            }

            var userTasks = await _context.Tasks
                .Where(x => x.OwnerId == userId)
                .ToListAsync();

            if (userTasks == null || userTasks.Count() == 0)
            {
                throw new ArgumentException("No event has been found");
            }

            return userTasks;
        }

        public async Task<IList<EventsTask>> GetUserTasksForAnEvent(int userId, int eventId)
        {
            if (userId == 0 || eventId == 0)
            {
                throw new ArgumentException($"userId {userId} or eventId {eventId} is 0");
            }

            var userTaskForAnEvent = await _context.Tasks
                .Where(x => x.EventId == eventId)
                .Where(x => x.OwnerId == userId)
                .ToListAsync();

            if (userTaskForAnEvent == null || userTaskForAnEvent.Count() == 0)
            {
                throw new ArgumentException("No event has been found");
            }

            return userTaskForAnEvent;
        }

        public async Task<IList<EventsTask>> GetTasksForEvent(int eventId)
        {
            if (eventId == 0)
            {
                throw new ArgumentException($"eventId {eventId} is not correct");
            }

            var tasksForAnEvent = await _context.Tasks
                .Where(x => x.EventId == eventId)
                .ToListAsync();

            if (tasksForAnEvent == null || tasksForAnEvent.Count() == 0)
            {
                throw new ArgumentException("No event has been found");
            }

            return tasksForAnEvent;
        }

        public async Task<EventsTask> GetTask(int taskId)
        {
            if (taskId == 0)
            {
                return new EventsTask();
            }

            var taskStatus = await _context.Tasks
                .Where(x => x.TaskId == taskId)
                .FirstOrDefaultAsync();

            return taskStatus;
        }

        public async Task<bool> CreateTask(EventsTask task)
        {
            if (task == null)
            {
                return false;
            }

            await _context.AddAsync(task);

            return await SaveChanges();
        }

        public async Task<bool> UpdateTask(EventsTask task)
        {
            if (task == null)
            {
                return false;
            }

            _context.Update(task);

            return await SaveChanges();
        }

        public async Task<bool> DeleteTask(EventsTask task)
        {
            if (task == null)
            {
                return false;
            }

            _context.Remove(task);

            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
