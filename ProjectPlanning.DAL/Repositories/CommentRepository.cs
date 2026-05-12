using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ProjectPlanningDbContext context) : base(context) { }

        public IEnumerable<Comment> GetByProject(int projectId)
        {
            return Set.Include(c => c.Author).Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CreatedUtc).ToList();
        }

        public IEnumerable<Comment> GetByTask(int taskId)
        {
            return Set.Include(c => c.Author).Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedUtc).ToList();
        }
    }
}
