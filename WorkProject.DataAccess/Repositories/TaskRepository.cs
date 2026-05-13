using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly WorkProjectDbContext _context;

        public TaskRepository(WorkProjectDbContext context)
        {
            _context = context;
        }

        public ProjectTask GetById(int taskId)
        {
            return _context.Tasks.Include(t => t.AssignedUser).FirstOrDefault(t => t.TaskId == taskId);
        }

        public IReadOnlyList<ProjectTask> GetByProject(int projectId)
        {
            return _context.Tasks
                .Include(t => t.AssignedUser)
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.SortOrder)
                .ToList();
        }

        public IReadOnlyList<TaskDependency> GetDependenciesByProject(int projectId)
        {
            return _context.TaskDependencies
                .Where(d => d.Predecessor.ProjectId == projectId && d.Successor.ProjectId == projectId)
                .ToList();
        }

        public void Add(ProjectTask task)
        {
            _context.Tasks.Add(task);
        }

        public void Update(ProjectTask task)
        {
            _context.Entry(task).State = EntityState.Modified;
        }

        public void Delete(int taskId)
        {
            var task = _context.Tasks.Find(taskId);
            if (task == null) return;

            var deps = _context.TaskDependencies
                .Where(d => d.PredecessorTaskId == taskId || d.SuccessorTaskId == taskId)
                .ToList();
            foreach (var d in deps)
                _context.TaskDependencies.Remove(d);

            _context.Tasks.Remove(task);
        }

        public void AddDependency(TaskDependency dependency)
        {
            _context.TaskDependencies.Add(dependency);
        }

        public void RemoveDependency(int dependencyId)
        {
            var d = _context.TaskDependencies.Find(dependencyId);
            if (d != null) _context.TaskDependencies.Remove(d);
        }

        public TaskDependency GetDependency(int dependencyId)
        {
            return _context.TaskDependencies.Find(dependencyId);
        }
    }
}
