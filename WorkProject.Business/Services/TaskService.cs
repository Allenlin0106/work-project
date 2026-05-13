using System;
using System.Collections.Generic;
using System.Linq;
using WorkProject.Business.Mapping;
using WorkProject.Business.Validation;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Repositories;
using WorkProject.Contracts.Services;
using WorkProject.Domain.Entities;
using WorkProject.Domain.Enums;

namespace WorkProject.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly Func<ITaskRepository> _taskRepoFactory;
        private readonly Func<ICommentRepository> _commentRepoFactory;
        private readonly Func<IUnitOfWork> _uowFactory;
        private readonly IAuthService _authService;

        public TaskService(
            Func<ITaskRepository> taskRepoFactory,
            Func<ICommentRepository> commentRepoFactory,
            Func<IUnitOfWork> uowFactory,
            IAuthService authService)
        {
            _taskRepoFactory = taskRepoFactory;
            _commentRepoFactory = commentRepoFactory;
            _uowFactory = uowFactory;
            _authService = authService;
        }

        public int CreateTask(NewTaskDto dto)
        {
            TaskValidator.Validate(dto);
            var user = RequireAuthenticated();

            using (var uow = _uowFactory())
            {
                var repo = _taskRepoFactory();
                var existing = repo.GetByProject(dto.ProjectId);
                var sortOrder = existing.Count == 0 ? 0 : existing.Max(t => t.SortOrder) + 1;
                var now = DateTime.UtcNow;
                var task = new ProjectTask
                {
                    ProjectId = dto.ProjectId,
                    ParentTaskId = dto.ParentTaskId,
                    Title = dto.Title,
                    Description = dto.Description,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    ProgressPercent = 0,
                    AssignedUserId = dto.AssignedUserId,
                    Status = TaskStatus.NotStarted,
                    IsMilestone = dto.IsMilestone,
                    SortOrder = sortOrder,
                    CreatedUtc = now,
                    UpdatedUtc = now
                };
                repo.Add(task);

                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
                return task.TaskId;
            }
        }

        public void UpdateTask(TaskDto dto)
        {
            TaskValidator.Validate(dto);
            var user = RequireAuthenticated();

            using (var uow = _uowFactory())
            {
                var repo = _taskRepoFactory();
                var entity = repo.GetById(dto.TaskId);
                if (entity == null) throw new InvalidOperationException("Task not found.");

                entity.Title = dto.Title;
                entity.Description = dto.Description;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.ProgressPercent = dto.ProgressPercent;
                entity.AssignedUserId = dto.AssignedUserId;
                entity.Status = dto.Status;
                entity.IsMilestone = dto.IsMilestone;
                entity.SortOrder = dto.SortOrder;
                entity.UpdatedUtc = DateTime.UtcNow;
                repo.Update(entity);

                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
            }
        }

        public void UpdateProgress(int taskId, int progressPercent)
        {
            if (progressPercent < 0 || progressPercent > 100)
                throw new InvalidOperationException("Progress must be between 0 and 100.");
            var user = RequireAuthenticated();

            using (var uow = _uowFactory())
            {
                var repo = _taskRepoFactory();
                var entity = repo.GetById(taskId);
                if (entity == null) throw new InvalidOperationException("Task not found.");
                entity.ProgressPercent = progressPercent;
                if (progressPercent == 100) entity.Status = TaskStatus.Completed;
                else if (progressPercent > 0 && entity.Status == TaskStatus.NotStarted)
                    entity.Status = TaskStatus.InProgress;
                entity.UpdatedUtc = DateTime.UtcNow;
                repo.Update(entity);

                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
            }
        }

        public void DeleteTask(int taskId)
        {
            var user = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _taskRepoFactory();
                repo.Delete(taskId);
                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
            }
        }

        public IReadOnlyList<TaskDto> ListByProject(int projectId)
        {
            RequireAuthenticated();
            var repo = _taskRepoFactory();
            var tasks = repo.GetByProject(projectId);
            var dependencies = repo.GetDependenciesByProject(projectId);
            var byTask = tasks.Select(t => t.ToDto()).ToList();
            var byTaskId = byTask.ToDictionary(t => t.TaskId);
            foreach (var d in dependencies)
            {
                if (byTaskId.TryGetValue(d.SuccessorTaskId, out var succ))
                    succ.Dependencies.Add(d.ToDto());
            }
            return byTask;
        }

        public int LinkDependency(int predecessorId, int successorId, DependencyType type, int lagDays)
        {
            if (predecessorId == successorId)
                throw new InvalidOperationException("A task cannot depend on itself.");
            var user = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _taskRepoFactory();
                var dep = new TaskDependency
                {
                    PredecessorTaskId = predecessorId,
                    SuccessorTaskId = successorId,
                    DependencyType = type,
                    LagDays = lagDays
                };
                repo.AddDependency(dep);
                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
                return dep.TaskDependencyId;
            }
        }

        public void UnlinkDependency(int dependencyId)
        {
            var user = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _taskRepoFactory();
                repo.RemoveDependency(dependencyId);
                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
            }
        }

        public int AddComment(int taskId, string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new InvalidOperationException("Comment body cannot be empty.");
            var user = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _commentRepoFactory();
                var comment = new Comment
                {
                    TaskId = taskId,
                    AuthorUserId = user.UserId,
                    Body = body,
                    CreatedUtc = DateTime.UtcNow
                };
                repo.Add(comment);
                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
                return comment.CommentId;
            }
        }

        public IReadOnlyList<CommentDto> ListComments(int taskId)
        {
            RequireAuthenticated();
            var repo = _commentRepoFactory();
            return repo.GetByTask(taskId).Select(c => c.ToDto()).ToList();
        }

        private UserPrincipal RequireAuthenticated()
        {
            var user = _authService.CurrentUser;
            if (user == null || !user.IsAuthenticated)
                throw new InvalidOperationException("Not authenticated.");
            return user;
        }
    }
}
