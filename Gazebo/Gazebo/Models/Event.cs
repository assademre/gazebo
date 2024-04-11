
namespace EventOrganizationApp.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int CreaterId { get; set; }
        public int EventTypeId { get; set; }
        public string EventName { get; set; }
        public decimal Budget { get; set; }
        public int CurrencyId { get; set; }
        public string Place { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime EventDate { get; set; }
    }
}
