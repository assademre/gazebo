using Gazebo.Data.Dto;

namespace Gazebo.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> AddComment(CommentDto comment);

        Task<IList<CommentDto>> GetCommentsByPostGroupId(int postGroupTypeId, int postGroupId);
    }
}
