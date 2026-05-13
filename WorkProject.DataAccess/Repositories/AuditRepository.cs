using System.Linq;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Repositories;
using WorkProject.DataAccess.Context;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly WorkProjectDbContext _context;

        public AuditRepository(WorkProjectDbContext context)
        {
            _context = context;
        }

        public void Add(AuditLog log)
        {
            _context.AuditLogs.Add(log);
        }

        public PagedResult<AuditLog> Query(AuditQueryDto query)
        {
            IQueryable<AuditLog> q = _context.AuditLogs;

            if (query.UserId.HasValue)
                q = q.Where(a => a.Who_UserId == query.UserId.Value);
            if (!string.IsNullOrWhiteSpace(query.EntityType))
                q = q.Where(a => a.EntityType == query.EntityType);
            if (!string.IsNullOrWhiteSpace(query.EntityId))
                q = q.Where(a => a.EntityId == query.EntityId);
            if (!string.IsNullOrWhiteSpace(query.Action))
                q = q.Where(a => a.Action == query.Action);
            if (query.FromUtc.HasValue)
                q = q.Where(a => a.WhenUtc >= query.FromUtc.Value);
            if (query.ToUtc.HasValue)
                q = q.Where(a => a.WhenUtc <= query.ToUtc.Value);

            var total = q.Count();
            var page = query.Page < 1 ? 1 : query.Page;
            var size = query.PageSize <= 0 ? 50 : query.PageSize;
            var items = q.OrderByDescending(a => a.WhenUtc)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return new PagedResult<AuditLog>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = size
            };
        }
    }
}
