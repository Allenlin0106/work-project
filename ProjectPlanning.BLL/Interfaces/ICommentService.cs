using System.Collections.Generic;
using ProjectPlanning.Entities.Dtos;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentDto> GetByProject(int projectId);
        IEnumerable<CommentDto> GetByTask(int taskId);
        int AddComment(int projectId, int? taskId, string body);
        void DeleteComment(int commentId);
    }
}
