using System.Linq;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;

namespace ProjectPlanning.BLL.Services
{
    public class GanttService : IGanttService
    {
        private readonly IProjectRepository _projects;
        private readonly ITaskRepository _tasks;

        public GanttService(IProjectRepository projects, ITaskRepository tasks)
        {
            _projects = projects;
            _tasks = tasks;
        }

        public GanttDataDto BuildGanttData(int projectId)
        {
            var project = _projects.GetById(projectId);
            if (project == null) return null;

            var tasks = _tasks.GetByProject(projectId).ToList();
            var deps = _tasks.GetDependenciesForProject(projectId).ToList();

            return new GanttDataDto
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Tasks = tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    ProjectId = t.ProjectId,
                    Name = t.Name,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    ProgressPercent = t.ProgressPercent,
                    AssigneeId = t.AssigneeId,
                    AssigneeDisplayName = t.Assignee != null ? t.Assignee.DisplayName : null,
                    ParentTaskId = t.ParentTaskId,
                    SortOrder = t.SortOrder
                }).ToList(),
                Dependencies = deps.Select(d => new TaskDependencyDto
                {
                    Id = d.Id,
                    PredecessorTaskId = d.PredecessorTaskId,
                    SuccessorTaskId = d.SuccessorTaskId,
                    Type = d.Type
                }).ToList()
            };
        }
    }
}
