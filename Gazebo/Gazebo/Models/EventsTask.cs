namespace EventOrganizationApp.Models
{
    public class EventsTask
    {
        public int TaskId { get; set; }
        public int EventId { get; set; }
        public string TaskName { get; set; }
        public decimal Budget { get; set; }
        public int CurrencyId { get; set; }
        public string Place { get; set; }
        public int StatusId { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime TaskDate { get; set; }
    }
}
