using System.Collections.Generic;
using WorkProject.Contracts.Dtos;
using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Services
{
    public interface ITaskService
    {
        int CreateTask(NewTaskDto dto);
        void UpdateTask(TaskDto dto);
        void UpdateProgress(int taskId, int progressPercent);
        void DeleteTask(int taskId);
        IReadOnlyList<TaskDto> ListByProject(int projectId);
        int LinkDependency(int predecessorId, int successorId, DependencyType type, int lagDays);
        void UnlinkDependency(int dependencyId);
        int AddComment(int taskId, string body);
        IReadOnlyList<CommentDto> ListComments(int taskId);
    }
}
