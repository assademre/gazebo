using Gazebo.Models;
using Gazebo.Models.Enums;

namespace Gazebo.Interfaces
{
    public interface INotificationRepository
    {
        Task<bool> CreateNewTaskNotification(int userId, string creatorName, string eventName);
        Task<bool> CreateNewCommentNotifications(int userId, int postGroupId, int postGroupTypeId);
        Task<List<Notification>> GetTaskNotifications(int userId);
        Task<bool> MakeNotificationRead(int userId, int notificationId);
    }
}
