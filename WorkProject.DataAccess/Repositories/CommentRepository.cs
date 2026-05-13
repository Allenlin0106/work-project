using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly WorkProjectDbContext _context;

        public CommentRepository(WorkProjectDbContext context)
        {
            _context = context;
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public IReadOnlyList<Comment> GetByTask(int taskId)
        {
            return _context.Comments
                .Include(c => c.Author)
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedUtc)
                .ToList();
        }

        public IReadOnlyList<Comment> GetByProject(int projectId)
        {
            return _context.Comments
                .Include(c => c.Author)
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CreatedUtc)
                .ToList();
        }
    }
}
