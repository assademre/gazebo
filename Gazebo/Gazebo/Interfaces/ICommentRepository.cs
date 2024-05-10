using Gazebo.Data.Dto;
using Gazebo.Models;

namespace Gazebo.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> AddComment(Comment comment);

        Task<IList<CommentDto>> GetCommentsByPostGroupId(int postGroupTypeId, int postGroupId, int pageNumber, int pageSize);
    }
}
