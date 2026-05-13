using System.Collections.Generic;
using WorkProject.Domain.Entities;

namespace WorkProject.Contracts.Repositories
{
    public interface ITaskRepository
    {
        ProjectTask GetById(int taskId);
        IReadOnlyList<ProjectTask> GetByProject(int projectId);
        IReadOnlyList<TaskDependency> GetDependenciesByProject(int projectId);
        void Add(ProjectTask task);
        void Update(ProjectTask task);
        void Delete(int taskId);
        void AddDependency(TaskDependency dependency);
        void RemoveDependency(int dependencyId);
        TaskDependency GetDependency(int dependencyId);
    }
}
