namespace EventOrganizationApp.Data.Dto
{
    public class EventTaskDto
    {
        public int TaskId { get; set; }
        public int EventId { get; set; }
        public string TaskName { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime TaskDate { get; set; }
    }
}
