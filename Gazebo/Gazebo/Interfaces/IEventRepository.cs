using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IEventRepository
    {
        Task<IList<Event>> GetEventsUserCreated(int userId);
        Task<string> GetStatusByEventId(int eventId);
        Task<bool> CreateEvent(Event newEvent);
        Task<bool> UpdateEvent(Event newEvent);
        Task<Event> GetEventByEventId(int eventId);
    }
}
