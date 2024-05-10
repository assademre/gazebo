using AutoMapper;
using EventOrganizationApp.Data;
using EventOrganizationApp.Data.Dto;
using EventOrganizationApp.Models;
using Gazebo.Data.Dto;
using Gazebo.Interfaces;
using Gazebo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gazebo.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        public IMapper _mapper;

        public CommentRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddComment(CommentDto comment)
        {
            if (comment == null)
            {
                return false;
            }

            var mappedComment = _mapper.Map<Comment>(comment);

            await _context.AddAsync(mappedComment);

            return await SaveChanges();
        }

        public async Task<IList<CommentDto>> GetCommentsByPostGroupId(int postGroupTypeId, int postGroupId)
        {
            if (postGroupTypeId == 0 || postGroupId == 0)
            {
                throw new ArgumentException($"postGroupTypeId {postGroupTypeId} or postGroupId {postGroupId} is not correct");
            }

            var commentList = await _context.Comments
                .Where(x => x.PostGroupId == postGroupId &&  x.PostGroupTypeId == postGroupTypeId)
                .ToListAsync();

            var mappedComment = _mapper.Map<IList<CommentDto>>(commentList);

            return mappedComment;
        }

        private async Task<bool> SaveChanges()
        {
            var savedData = await _context.SaveChangesAsync();

            return savedData > 0;
        }
    }
}
