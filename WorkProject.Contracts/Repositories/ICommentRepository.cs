using System.Collections.Generic;
using WorkProject.Domain.Entities;

namespace WorkProject.Contracts.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        IReadOnlyList<Comment> GetByTask(int taskId);
        IReadOnlyList<Comment> GetByProject(int projectId);
    }
}
