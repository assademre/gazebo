using EventOrganizationApp.Models;
using System.Diagnostics.Eventing.Reader;

namespace EventOrganizationApp.Interfaces.Users
{
    public interface IEventTaskRepository
    {
        IList<EventsTask> GetAllUserTasks(int userId);
        IList<EventsTask> GetUserTasksForAnEvent(int userId, int eventId);
    }
}
