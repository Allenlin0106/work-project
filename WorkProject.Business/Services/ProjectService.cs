using System;
using System.Collections.Generic;
using WorkProject.Business.Mapping;
using WorkProject.Business.Validation;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Repositories;
using WorkProject.Contracts.Services;
using WorkProject.Domain.Entities;
using WorkProject.Domain.Enums;

namespace WorkProject.Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly Func<IProjectRepository> _projectRepoFactory;
        private readonly Func<ITaskRepository> _taskRepoFactory;
        private readonly Func<IUnitOfWork> _uowFactory;
        private readonly IAuthService _authService;

        public ProjectService(
            Func<IProjectRepository> projectRepoFactory,
            Func<ITaskRepository> taskRepoFactory,
            Func<IUnitOfWork> uowFactory,
            IAuthService authService)
        {
            _projectRepoFactory = projectRepoFactory;
            _taskRepoFactory = taskRepoFactory;
            _uowFactory = uowFactory;
            _authService = authService;
        }

        public int CreateProject(NewProjectDto dto)
        {
            ProjectValidator.Validate(dto);
            var user = RequireAuthenticated();

            using (var uow = _uowFactory())
            {
                var repo = _projectRepoFactory();
                var now = DateTime.UtcNow;
                var project = new Project
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Status = ProjectStatus.Planning,
                    OwnerUserId = user.UserId,
                    CreatedUtc = now,
                    UpdatedUtc = now
                };
                repo.Add(project);
                repo.AddMember(new ProjectMember
                {
                    Project = project,
                    UserId = user.UserId,
                    Role = ProjectRole.Owner,
                    JoinedUtc = now
                });

                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
                return project.ProjectId;
            }
        }

        public void UpdateProject(ProjectDto dto)
        {
            ProjectValidator.Validate(dto);
            var user = RequireAuthenticated();

            using (var uow = _uowFactory())
            {
                var repo = _projectRepoFactory();
                var entity = repo.GetById(dto.ProjectId);
                if (entity == null) throw new InvalidOperationException("Project not found.");

                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.Status = dto.Status;
                entity.UpdatedUtc = DateTime.UtcNow;
                repo.Update(entity);

                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
            }
        }

        public void DeleteProject(int projectId)
        {
            var user = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _projectRepoFactory();
                repo.Delete(projectId);
                uow.SetCurrentUser(user.UserId);
                uow.SaveChanges();
            }
        }

        public ProjectDto Get(int projectId)
        {
            RequireAuthenticated();
            var repo = _projectRepoFactory();
            var entity = repo.GetById(projectId, includeMembers: true);
            return entity?.ToDto();
        }

        public IReadOnlyList<ProjectSummaryDto> ListForCurrentUser()
        {
            var user = RequireAuthenticated();
            var projectRepo = _projectRepoFactory();
            var taskRepo = _taskRepoFactory();
            var result = new List<ProjectSummaryDto>();

            foreach (var project in projectRepo.GetForUser(user.UserId))
            {
                var tasks = taskRepo.GetByProject(project.ProjectId);
                var completed = 0;
                double sum = 0;
                foreach (var t in tasks)
                {
                    if (t.Status == TaskStatus.Completed) completed++;
                    sum += t.ProgressPercent;
                }
                var avg = tasks.Count == 0 ? 0 : sum / tasks.Count;
                result.Add(project.ToSummaryDto(tasks.Count, completed, avg));
            }
            return result;
        }

        public void AddMember(int projectId, int userId, ProjectRole role)
        {
            var current = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _projectRepoFactory();
                repo.AddMember(new ProjectMember
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Role = role,
                    JoinedUtc = DateTime.UtcNow
                });
                uow.SetCurrentUser(current.UserId);
                uow.SaveChanges();
            }
        }

        public void RemoveMember(int projectMemberId)
        {
            var current = RequireAuthenticated();
            using (var uow = _uowFactory())
            {
                var repo = _projectRepoFactory();
                repo.RemoveMember(projectMemberId);
                uow.SetCurrentUser(current.UserId);
                uow.SaveChanges();
            }
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
