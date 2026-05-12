using System.Collections.Generic;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskDto> GetByProject(int projectId);
        int CreateTask(TaskEditDto dto);
        void UpdateTask(TaskEditDto dto);
        void DeleteTask(int taskId);
        int SetDependency(int predecessorId, int successorId, DependencyType type);
        void RemoveDependency(int dependencyId);
    }
}
