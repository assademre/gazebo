﻿namespace EventOrganizationApp.Models
{
    public class EventMember
    {
        public int EventMembersId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
