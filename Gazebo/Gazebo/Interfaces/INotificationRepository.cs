using Gazebo.Models;

namespace Gazebo.Interfaces
{
    public interface INotificationRepository
    {
        Task<bool> CreateNewTaskNotification(int userId, string creatorName, string eventName);
        Task<List<Notification>> GetTaskNotifications(int userId);
        Task<bool> MakeNotificationRead(int userId, int notificationId);
    }
}
