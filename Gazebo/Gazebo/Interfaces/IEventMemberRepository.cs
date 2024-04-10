using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IEventMemberRepository
    {
        Task<bool> AddEventMember(EventMember eventMember);
        Task<bool> IsEventMemberAlreadyAdded(int eventId, int userId);
    }
}
