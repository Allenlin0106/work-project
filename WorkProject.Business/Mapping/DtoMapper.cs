using System.Linq;
using WorkProject.Contracts.Dtos;
using WorkProject.Domain.Entities;

namespace WorkProject.Business.Mapping
{
    public static class DtoMapper
    {
        public static ProjectDto ToDto(this Project entity)
        {
            if (entity == null) return null;
            return new ProjectDto
            {
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                OwnerUserId = entity.OwnerUserId,
                OwnerDisplayName = entity.Owner?.DisplayName,
                Members = entity.Members?.Select(m => m.ToDto()).ToList() ?? new System.Collections.Generic.List<ProjectMemberDto>()
            };
        }

        public static ProjectMemberDto ToDto(this ProjectMember entity)
        {
            if (entity == null) return null;
            return new ProjectMemberDto
            {
                ProjectMemberId = entity.ProjectMemberId,
                ProjectId = entity.ProjectId,
                UserId = entity.UserId,
                DisplayName = entity.User?.DisplayName,
                UserName = entity.User?.UserName,
                Role = entity.Role,
                JoinedUtc = entity.JoinedUtc
            };
        }

        public static TaskDto ToDto(this ProjectTask entity)
        {
            if (entity == null) return null;
            return new TaskDto
            {
                TaskId = entity.TaskId,
                ProjectId = entity.ProjectId,
                ParentTaskId = entity.ParentTaskId,
                Title = entity.Title,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ProgressPercent = entity.ProgressPercent,
                AssignedUserId = entity.AssignedUserId,
                AssignedDisplayName = entity.AssignedUser?.DisplayName,
                Status = entity.Status,
                IsMilestone = entity.IsMilestone,
                SortOrder = entity.SortOrder
            };
        }

        public static TaskDependencyDto ToDto(this TaskDependency entity)
        {
            if (entity == null) return null;
            return new TaskDependencyDto
            {
                TaskDependencyId = entity.TaskDependencyId,
                PredecessorTaskId = entity.PredecessorTaskId,
                SuccessorTaskId = entity.SuccessorTaskId,
                DependencyType = entity.DependencyType,
                LagDays = entity.LagDays
            };
        }

        public static CommentDto ToDto(this Comment entity)
        {
            if (entity == null) return null;
            return new CommentDto
            {
                CommentId = entity.CommentId,
                TaskId = entity.TaskId,
                ProjectId = entity.ProjectId,
                AuthorUserId = entity.AuthorUserId,
                AuthorDisplayName = entity.Author?.DisplayName,
                Body = entity.Body,
                CreatedUtc = entity.CreatedUtc
            };
        }

        public static AuditLogDto ToDto(this AuditLog entity)
        {
            if (entity == null) return null;
            return new AuditLogDto
            {
                AuditLogId = entity.AuditLogId,
                Who_UserId = entity.Who_UserId,
                WhoDisplayName = entity.Who?.DisplayName,
                WhenUtc = entity.WhenUtc,
                Action = entity.Action,
                EntityType = entity.EntityType,
                EntityId = entity.EntityId,
                BeforeJson = entity.BeforeJson,
                AfterJson = entity.AfterJson,
                CorrelationId = entity.CorrelationId
            };
        }

        public static ProjectSummaryDto ToSummaryDto(this Project entity, int taskCount, int completed, double avgProgress)
        {
            return new ProjectSummaryDto
            {
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                TaskCount = taskCount,
                CompletedTaskCount = completed,
                ProgressPercent = avgProgress
            };
        }
    }
}
