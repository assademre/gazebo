using Gazebo.Models.Enums;

namespace Gazebo.Models
{
    public class Comment
    {
        public int CommentId { get; set; } 
        public byte PostGroupTypeId { get; set; }
        public int PostGroupId { get; set; }
        public int CommentOwnerId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
