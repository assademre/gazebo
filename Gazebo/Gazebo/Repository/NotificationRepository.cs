using EventOrganizationApp.Data;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;

        public NotificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetTaskNotifications(int userId)
        {
            if (userId == 0)
            {
                return null;
            }


            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return notifications;
        }

        public async Task<bool> CreateNewTaskNotification(int userId, string creatorName, string eventName)
        {
            if (userId == 0)
            {
                return false;
            }

            var notification = new Notification
            {
                UserId = userId,
                Subject = "New Task",
                Body = $"{creatorName} assigned you a new task for {eventName}",
                CreatedDate = DateTime.UtcNow,
                IsRead = false
            };

            await _context.Notifications.AddAsync(notification);

            return await SaveChanges();
        }

        public async Task<bool> MakeNotificationRead(int userId, int notificationId)
        {
            if (userId == 0 || notificationId == 0)
            {
                return false;
            }

            var notification = await _context.Notifications
                .Where(x => x.UserId == userId && x.NotificationId == notificationId)
                .FirstOrDefaultAsync();

            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;

            _context.Notifications.Update(notification);

            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
