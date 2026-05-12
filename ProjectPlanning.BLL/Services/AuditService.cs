using System;
using System.Linq;
using ProjectPlanning.BLL.Interfaces;
using ProjectPlanning.Common.Auditing;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _audit;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserAccessor _userInfo;

        public AuditService(IAuditLogRepository audit, IUnitOfWork uow, ICurrentUserAccessor userInfo)
        {
            _audit = audit;
            _uow = uow;
            _userInfo = userInfo;
        }

        public void LogLogin(string windowsAccount)
        {
            WriteSimple(AuditAction.Login, null, null, windowsAccount, null);
        }

        public void LogLogout(string windowsAccount)
        {
            WriteSimple(AuditAction.Logout, null, null, windowsAccount, null);
        }

        public void LogView(string entityType, string entityKey, string notes = null)
        {
            WriteSimple(AuditAction.View, entityType, entityKey, _userInfo.WindowsAccount, notes);
        }

        public void LogExport(string entityType, string entityKey, string format)
        {
            WriteSimple(AuditAction.Export, entityType, entityKey, _userInfo.WindowsAccount, "Format=" + format);
        }

        public PagedResult<AuditLogDto> Search(AuditQuery query, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var q = _audit.Search(query);
            var total = q.Count();
            var items = q.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    TimestampUtc = a.TimestampUtc,
                    WindowsAccount = a.WindowsAccount,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityKey = a.EntityKey,
                    BeforeJson = a.BeforeJson,
                    AfterJson = a.AfterJson,
                    IpAddress = a.IpAddress,
                    ControllerAction = a.ControllerAction,
                    Notes = a.Notes
                }).ToList();

            return new PagedResult<AuditLogDto>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        private void WriteSimple(string action, string entityType, string entityKey, string account, string notes)
        {
            var log = new AuditLog
            {
                TimestampUtc = DateTime.UtcNow,
                WindowsAccount = account ?? "system",
                UserId = _userInfo != null ? _userInfo.UserId : null,
                Action = action,
                EntityType = entityType,
                EntityKey = entityKey,
                IpAddress = _userInfo != null ? _userInfo.IpAddress : null,
                UserAgent = _userInfo != null ? _userInfo.UserAgent : null,
                ControllerAction = _userInfo != null ? _userInfo.ControllerAction : null,
                Notes = notes
            };
            _audit.Add(log);
            _uow.SaveChanges();
        }
    }
}
