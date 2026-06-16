using System.Collections.Generic;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        IEnumerable<TaskItem> GetByProject(int projectId);
        IEnumerable<TaskDependency> GetDependenciesForProject(int projectId);
        void AddDependency(TaskDependency dependency);
        void RemoveDependency(TaskDependency dependency);
        TaskDependency GetDependency(int id);
    }
}
