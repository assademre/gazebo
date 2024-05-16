using AutoMapper;
using EventOrganizationApp.Data;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.EntityFrameworkCore;

namespace Gazebo.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public IMapper _mapper;

        public CommentRepository(DataContext context, IUserRepository userRepository,  IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            if (comment == null)
            {
                return false;
            }

            await _context.AddAsync(comment);

            return await SaveChanges();
        }

        public async Task<PaginatedCommentsDto> GetCommentsByPostGroupId(int postGroupTypeId, int postGroupId, int pageNumber, int pageSize)
        {
            if (postGroupTypeId == 0 || postGroupId == 0)
            {
                throw new ArgumentException($"postGroupTypeId {postGroupTypeId} or postGroupId {postGroupId} is not correct");
            }

            var query = _context.Comments
                .Where(x => x.PostGroupId == postGroupId && x.PostGroupTypeId == postGroupTypeId)
                .OrderByDescending(x => x.CommentDate);

            var totalComments = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalComments / pageSize);

            var commentList = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var commentDtoList = new List<CommentDto>();

            foreach (var comment in commentList)
            {
                var commentDto = _mapper.Map<CommentDto>(comment);
                var userId = comment.CommentOwnerId;
                var username = _userRepository.GetUserInfo(userId).Username;
                commentDto.CommentOwnerName = username;
                commentDtoList.Add(commentDto);
            }

            return new PaginatedCommentsDto
            {
                Comments = commentDtoList,
                TotalPages = totalPages
            };
        }

        private async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
