﻿using EventOrganizationApp.Models;

namespace Gazebo.Interfaces
{
    public interface IEventMemberRepository
    {
        Task<bool> AddEventMember(EventMember eventMember);
        Task<bool> IsUserMember(int eventId, int userId);
        Task<bool> IsUserAdmin(int eventId, int userId);
        Task<List<int>> GetAdminEvents(int userId);
        Task<List<int>> GetUserEvents(int userId);
    }
}
