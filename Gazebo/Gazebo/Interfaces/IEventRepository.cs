using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IEventRepository
    {
        IList<Event> GetEventsUserCreated(int userId);
        string GetStatusByEventId(int eventId);
        bool CreateEvent(Event newEvent);
    }
}
