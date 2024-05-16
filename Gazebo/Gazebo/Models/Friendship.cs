namespace Gazebo.Models
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int FriendshipStatusId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
