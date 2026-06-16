using System;
using System.Collections.Generic;
using System.Linq;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Enums;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _tasks;
        private readonly IUnitOfWork _uow;

        public TaskService(ITaskRepository tasks, IUnitOfWork uow)
        {
            _tasks = tasks;
            _uow = uow;
        }

        public IEnumerable<TaskDto> GetByProject(int projectId)
        {
            return _tasks.GetByProject(projectId).Select(Map).ToList();
        }

        public int CreateTask(TaskEditDto dto)
        {
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("End date must be on or after start date.");

            var now = DateTime.UtcNow;
            var entity = new TaskItem
            {
                ProjectId = dto.ProjectId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ProgressPercent = dto.ProgressPercent,
                AssigneeId = dto.AssigneeId,
                ParentTaskId = dto.ParentTaskId,
                SortOrder = dto.SortOrder,
                CreatedUtc = now,
                ModifiedUtc = now
            };
            _tasks.Add(entity);
            _uow.SaveChanges();
            return entity.Id;
        }

        public void UpdateTask(TaskEditDto dto)
        {
            var entity = _tasks.GetById(dto.Id);
            if (entity == null) throw new InvalidOperationException("Task not found.");
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("End date must be on or after start date.");

            entity.Name = dto.Name;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.ProgressPercent = dto.ProgressPercent;
            entity.AssigneeId = dto.AssigneeId;
            entity.ParentTaskId = dto.ParentTaskId;
            entity.SortOrder = dto.SortOrder;
            entity.ModifiedUtc = DateTime.UtcNow;
            _uow.SaveChanges();
        }

        public void DeleteTask(int taskId)
        {
            var entity = _tasks.GetById(taskId);
            if (entity == null) return;
            _tasks.Remove(entity);
            _uow.SaveChanges();
        }

        public int SetDependency(int predecessorId, int successorId, DependencyType type)
        {
            if (predecessorId == successorId)
                throw new InvalidOperationException("A task cannot depend on itself.");
            var dep = new TaskDependency
            {
                PredecessorTaskId = predecessorId,
                SuccessorTaskId = successorId,
                Type = type
            };
            _tasks.AddDependency(dep);
            _uow.SaveChanges();
            return dep.Id;
        }

        public void RemoveDependency(int dependencyId)
        {
            var dep = _tasks.GetDependency(dependencyId);
            if (dep == null) return;
            _tasks.RemoveDependency(dep);
            _uow.SaveChanges();
        }

        private static TaskDto Map(TaskItem t)
        {
            return new TaskDto
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
            };
        }
    }
}
