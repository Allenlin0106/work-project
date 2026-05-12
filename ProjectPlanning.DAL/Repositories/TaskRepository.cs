using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(ProjectPlanningDbContext context) : base(context) { }

        public IEnumerable<TaskItem> GetByProject(int projectId)
        {
            return Set
                .Include(t => t.Assignee)
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.SortOrder)
                .ToList();
        }

        public IEnumerable<TaskDependency> GetDependenciesForProject(int projectId)
        {
            return Context.TaskDependencies
                .Where(d => d.PredecessorTask.ProjectId == projectId && d.SuccessorTask.ProjectId == projectId)
                .ToList();
        }

        public void AddDependency(TaskDependency dependency) => Context.TaskDependencies.Add(dependency);

        public void RemoveDependency(TaskDependency dependency) => Context.TaskDependencies.Remove(dependency);

        public TaskDependency GetDependency(int id) => Context.TaskDependencies.Find(id);
    }
}
