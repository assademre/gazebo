﻿using EventOrganizationApp.Data;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Gazebo.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public FriendshipRepository(DataContext context, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<bool> RespondFriendshipRequest(int userId, int senderId, int respondId)
        {
            if (userId == 0 || senderId == 0)
            {
                return false;
            }

            var friendship = await _context.Friendships
                .Where(x => x.SenderId == senderId && x.ReceiverId == userId)
                .FirstOrDefaultAsync();

            if (friendship == null)
            {
                return false;
            }

            friendship.FriendshipStatusId = respondId;

            _context.Update(friendship);

            return await SaveChanges();
        }

        public async Task<IList<FriendshipDto>> GetFriends(int userId)
        {
            if (userId == 0)
            {
                return new List<FriendshipDto>();
            }

            var friendsList = await _context.Friendships
                .Where(x => (x.SenderId == userId || x.ReceiverId == userId) && x.FriendshipStatusId == (int)FriendshipStatus.Accepted)
                .ToListAsync();

            var friendshipListDto = new List<FriendshipDto>();

            foreach (var friend in friendsList)
            {
                var friendId = friend.SenderId == userId ? friend.ReceiverId : friend.SenderId;
                var userInfo = _userRepository.GetUserInfo(friendId);
                friendshipListDto.Add(new FriendshipDto
                {
                    FriendName = userInfo.Username
                });
            }

            return friendshipListDto;
        }

        public async Task<bool> RemoveFriend(int userId, int friendId)
        {
            if (userId == 0 || friendId == 0)
            {
                return false;
            }

            var friendship = await _context.Friendships
                .Where(x => (x.SenderId == friendId && x.ReceiverId == userId) || (x.SenderId == userId && x.ReceiverId == friendId))
                .FirstOrDefaultAsync();

            if (friendship == null)
            {
                return false;
            }

            friendship.FriendshipStatusId = (int)FriendshipStatus.Removed;

            _context.Update(friendship);

            return await SaveChanges();
        }

        public async Task<bool> SendFriendshipRequest(int userId, int receiverId)
        {
            if (userId == 0 || receiverId == 0)
            {
                return false;
            }

            var friendship = new Friendship()
            {
                SenderId = userId,
                ReceiverId = receiverId,
                FriendshipStatusId = (int)FriendshipStatus.Pending,
                UpdateDate = DateTime.UtcNow
            };

            await _context.AddAsync(friendship);

            return await SaveChanges();
        }

        private async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}