using EventOrganizationApp.Models.Enums;

namespace EventOrganizationApp.Models
{
    public class EventMember
    {
        public int EventMemberId { get; set; }
        public EventType Event { get; set; }
        public User User { get; set; }
        public EventsTask Tasks { get; set; }
    }
}
