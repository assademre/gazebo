using EventOrganizationApp.Data;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Gazebo.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IEventTaskRepository _eventTaskRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventMemberRepository _eventMemberRepository;

        public NotificationRepository(DataContext context,
            IUserRepository userRepository,
            IEventTaskRepository eventTaskRepository,
            IEventRepository eventRepository,
            IEventMemberRepository eventMemberRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _eventTaskRepository = eventTaskRepository;
            _eventRepository = eventRepository;
            _eventMemberRepository = eventMemberRepository;
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

        public async Task<bool> CreateNewCommentNotifications(int userId, int postGroupId, int postGroupTypeId)
        {
            string postGroupTypeName = string.Empty;
            string postGroupName = string.Empty;
            List<int> memberList = new List<int>();


            if (userId == 0)
            {
                return false;
            }
            
            if (postGroupTypeId == (int)PostGroup.Event)
            {
                memberList = await _eventMemberRepository.GetMemberListForAnEvent(postGroupId);
                postGroupTypeName = PostGroup.Event.ToString();
                var eventInfo = await _eventRepository.GetEventByEventId(postGroupId);
                postGroupName = eventInfo.EventName;
            }

            else if (postGroupTypeId == (int)PostGroup.Task)
            {
                var taskInfo = await _eventTaskRepository.GetTask(postGroupId);
                memberList = new List<int>
                {
                    taskInfo.OwnerId
                };
                postGroupTypeName = PostGroup.Task.ToString();
                postGroupName = taskInfo.TaskName;
            }

            var commentOwner = _userRepository.GetUserInfo(userId).Username;

            foreach (var member in memberList)
            {
                var response = await CreateNewCommentNotification(commentOwner, member, postGroupName, postGroupTypeName);
                
                if (!response)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> CreateNewCommentNotification(string commentOwner, int member, string postGroupName, string postGroupTypeName)
        {
            var notification = new Notification
            {
                UserId = member,
                Subject = "New Comment",
                Body = $"{commentOwner} added a new comment under {postGroupName} {postGroupTypeName}",
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
