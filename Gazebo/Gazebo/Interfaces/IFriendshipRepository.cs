using Gazebo.Data.Dto;

namespace Gazebo.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<IList<FriendshipDto>> GetFriends(int userId);
        Task<bool> SendFriendshipRequest(int userId, int receiverId);
        Task<bool> RespondFriendshipRequest(int userId, int senderId, int respondId);
        Task<IList<FriendshipDto>> FriendshipRequests(int userId);
        Task<bool> RemoveFriend(int userId, int friendId);
    }
}
