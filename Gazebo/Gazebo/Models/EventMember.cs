namespace EventOrganizationApp.Models
{
    public class EventMember
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
