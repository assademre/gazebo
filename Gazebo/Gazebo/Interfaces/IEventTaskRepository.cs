using EventOrganizationApp.Models;
using System.Diagnostics.Eventing.Reader;

namespace Gazebo.Interfaces
{
    public interface IEventTaskRepository
    {
        IList<EventsTask> GetAllUserTasks(int userId);
        IList<EventsTask> GetUserTasksForAnEvent(int userId, int eventId);
        IList<EventsTask> GetTasksForEvent(int eventId);
        string GetStatusByTaskId(int taskId);
        bool CreateTask(EventsTask task);
    }
}
