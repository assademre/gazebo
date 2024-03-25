using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;

namespace EventOrganizationApp.Interfaces.Users
{
    public interface IEventRepository
    {
        IList<Event> GetEventsUserCreated(int userId);
        string GetStatusByEventId(int eventId);
        bool CreateEvent(Event newEvent);
    }
}
