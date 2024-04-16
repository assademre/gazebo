using System.ComponentModel.DataAnnotations;

namespace EventOrganizationApp.Data.Dto
{
    public class EventTaskDto
    {
        public int TaskId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Task name must be minimum 5 characters")]
        [MaxLength(50, ErrorMessage = "Task name cannot access 50 characters")]
        public string TaskName { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Place name must be minimum 5 characters")]
        [MaxLength(50, ErrorMessage = "Place name cannot access 50 characters")]
        public string Place { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }    
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime TaskDate { get; set; }
    }
}
