using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;

namespace EventOrganizationApp.Data.Dto
{
    public class EventDto
    {
        public int EventId { get; set; }
        public int CreaterId { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime EventDate { get; set; }
    }
}
