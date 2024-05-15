namespace Gazebo.Data.Dto
{
    public class PaginatedCommentsDto
    {
        public IList<CommentDto> Comments { get; set; }
        public int TotalPages { get; set; }
    }
}
