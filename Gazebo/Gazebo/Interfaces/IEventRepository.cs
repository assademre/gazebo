using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IEventRepository
    {
        Task<IList<Event>> GetEventsUserCreated(int userId);
        Task<int> GetEventIdByEventNameAndUserId(int userId, string eventName);
        Task<bool> CreateEvent(Event newEvent);
        Task<bool> UpdateEvent(Event newEvent);
        Task<Event> GetEventByEventId(int eventId);
        Task<List<Event>> GetEventsByEventsId(List<int> eventIds);
        Task<bool> DeleteEvent(Event eventInfo);
    }
}
