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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projects;
        private readonly IProjectMemberRepository _members;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserProvider _currentUser;

        public ProjectService(IProjectRepository projects, IProjectMemberRepository members,
            IUnitOfWork uow, ICurrentUserProvider currentUser)
        {
            _projects = projects;
            _members = members;
            _uow = uow;
            _currentUser = currentUser;
        }

        public IEnumerable<ProjectListDto> ListForCurrentUser()
        {
            var user = _currentUser.GetOrProvision();
            return _projects.ListForUser(user.Id)
                .Select(p => new ProjectListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    OwnerDisplayName = p.Owner.DisplayName,
                    TaskCount = p.Tasks.Count,
                    MemberCount = p.Members.Count
                })
                .ToList();
        }

        public ProjectDetailDto Get(int id)
        {
            var p = _projects.GetWithTasks(id);
            if (p == null) return null;
            return new ProjectDetailDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                OwnerId = p.OwnerId,
                OwnerDisplayName = p.Owner != null ? p.Owner.DisplayName : null,
                Members = p.Members
                    .Select(m => new MemberDto
                    {
                        Id = m.Id,
                        ProjectId = m.ProjectId,
                        UserId = m.UserId,
                        WindowsAccount = m.User != null ? m.User.WindowsAccount : null,
                        DisplayName = m.User != null ? m.User.DisplayName : null,
                        Role = m.Role,
                        AddedUtc = m.AddedUtc
                    }).ToList(),
                Tasks = p.Tasks
                    .OrderBy(t => t.SortOrder)
                    .Select(t => new TaskDto
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
                    }).ToList()
            };
        }

        public int Create(ProjectEditDto dto)
        {
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("End date must be on or after start date.");

            var owner = _currentUser.GetOrProvision();
            var now = DateTime.UtcNow;
            var entity = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                OwnerId = owner.Id,
                CreatedUtc = now,
                ModifiedUtc = now
            };
            _projects.Add(entity);
            _uow.SaveChanges();

            _members.Add(new ProjectMember
            {
                ProjectId = entity.Id,
                UserId = owner.Id,
                Role = ProjectRole.Owner,
                AddedUtc = now
            });
            _uow.SaveChanges();
            return entity.Id;
        }

        public void Update(ProjectEditDto dto)
        {
            var entity = _projects.GetById(dto.Id);
            if (entity == null) throw new InvalidOperationException("Project not found.");
            if (dto.EndDate < dto.StartDate)
                throw new InvalidOperationException("End date must be on or after start date.");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Status = dto.Status;
            entity.ModifiedUtc = DateTime.UtcNow;
            _uow.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _projects.GetById(id);
            if (entity == null) return;
            _projects.Remove(entity);
            _uow.SaveChanges();
        }
    }
}
