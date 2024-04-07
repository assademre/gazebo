﻿using EventOrganizationApp.Data;
using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Gazebo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly DataContext _context;
        public EventRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IList<Event>> GetEventsUserCreated(int userId)
        {
            if (userId == 0)
            {
                return null;
            }

            var userEvents = await _context.Events
                .Where(x => x.CreaterId == userId)
                .ToListAsync();

            if (userEvents == null || userEvents.Count() == 0)
            {
                return null;
            }

            return userEvents;
        }

        public async Task<string> GetStatusByEventId(int eventId)
        {
            if (eventId == 0)
            {
                return string.Empty;
            }

            var eventStatus = await _context.Events
                .Where(x => x.EventId == eventId)
                .Select(x => ((Status)x.StatusId).ToString())
                .FirstOrDefaultAsync() ?? string.Empty;

            return eventStatus;
        }

        public async Task<bool> CreateEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                return false;
            }

            await _context.AddAsync(newEvent);

            return await SaveChanges();
        }

        public async Task<bool> UpdateEvent(Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                return false;
            }

            _context.Update(updatedEvent);

            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }

        public async Task<Event> GetEventByEventId(int eventId)
        {
            if (eventId == 0)
            {
                return null;
            }

            var result = await _context.Events
                .Where(x => x.EventId == eventId)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
