using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace Gazebo.Interfaces
{
    public interface IEventTaskRepository
    {
        Task<IList<EventsTask>> GetAllUserTasks(int userId);
        Task<IList<EventsTask>> GetUserTasksForAnEvent(int userId, int eventId);
        Task<IList<EventsTask>> GetTasksForEvent(int eventId);
        Task<int> GetEventIdByTaskId (int taskId);
        Task<EventsTask> GetTask(int taskId);
        Task<bool> CreateTask(EventsTask task);
        Task<bool> UpdateTask(EventsTask task);
        Task<bool> DeleteTask(EventsTask task);
    }
}
