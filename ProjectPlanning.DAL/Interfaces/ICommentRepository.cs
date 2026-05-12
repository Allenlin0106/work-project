using System.Collections.Generic;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetByProject(int projectId);
        IEnumerable<Comment> GetByTask(int taskId);
    }
}
