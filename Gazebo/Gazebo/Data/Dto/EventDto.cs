﻿using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventOrganizationApp.Data.Dto
{
    public class EventDto
    {
        public int EventId { get; set; }
        public int CreaterId { get; set; }
        public string EventType { get; set; }

        [Required]
        [MinLength(5, ErrorMessage ="Event name must be minimum 5 characters")]
        [MaxLength(50, ErrorMessage = "Event name cannot access 50 characters")]
        public string EventName { get; set; }
        public decimal Budget { get; set; }
        public string Currency { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Place name must be minimum 5 characters")]
        [MaxLength(50, ErrorMessage = "Place name cannot access 50 characters")]
        public string Place { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime EventDate { get; set; }
    }
}
