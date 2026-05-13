using System;
using System.Collections.Generic;
using WorkProject.Business.Mapping;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Repositories;
using WorkProject.Contracts.Services;
using WorkProject.Domain.Entities;
using WorkProject.Domain.Enums;

namespace WorkProject.Business.Services
{
    public class AuditService : IAuditService
    {
        private readonly Func<IAuditRepository> _auditRepoFactory;
        private readonly Func<IUnitOfWork> _uowFactory;

        public AuditService(Func<IAuditRepository> auditRepoFactory, Func<IUnitOfWork> uowFactory)
        {
            _auditRepoFactory = auditRepoFactory;
            _uowFactory = uowFactory;
        }

        public PagedResult<AuditLogDto> Query(AuditQueryDto query)
        {
            var repo = _auditRepoFactory();
            var page = repo.Query(query);
            var items = new List<AuditLogDto>();
            foreach (var item in page.Items) items.Add(item.ToDto());
            return new PagedResult<AuditLogDto>
            {
                Items = items,
                TotalCount = page.TotalCount,
                Page = page.Page,
                PageSize = page.PageSize
            };
        }

        public void RecordLogin(int? userId, string userName, bool success)
        {
            WriteStandalone(userId, success ? AuditAction.Login : AuditAction.LoginFailed,
                "User", userId?.ToString(), afterJson: $"{{\"UserName\":\"{Escape(userName)}\"}}");
        }

        public void RecordLogout(int userId, string userName)
        {
            WriteStandalone(userId, AuditAction.Logout, "User", userId.ToString(),
                afterJson: $"{{\"UserName\":\"{Escape(userName)}\"}}");
        }

        public void RecordExport(int userId, string what)
        {
            WriteStandalone(userId == 0 ? (int?)null : userId, AuditAction.Export, "Export", what,
                afterJson: $"{{\"What\":\"{Escape(what)}\"}}");
        }

        private void WriteStandalone(int? userId, string action, string entityType, string entityId, string afterJson)
        {
            using (var uow = _uowFactory())
            {
                var repo = _auditRepoFactory();
                repo.Add(new AuditLog
                {
                    Who_UserId = userId,
                    WhenUtc = DateTime.UtcNow,
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    AfterJson = afterJson,
                    CorrelationId = Guid.NewGuid()
                });
                uow.SaveChanges();
            }
        }

        private static string Escape(string s) =>
            string.IsNullOrEmpty(s) ? string.Empty : s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
