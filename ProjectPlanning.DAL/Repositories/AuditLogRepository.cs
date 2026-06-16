using System.Linq;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Repositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ProjectPlanningDbContext context) : base(context) { }

        public IQueryable<AuditLog> Search(AuditQuery query)
        {
            var q = Set.AsQueryable();
            if (query == null) return q;

            if (!string.IsNullOrWhiteSpace(query.WindowsAccount))
                q = q.Where(a => a.WindowsAccount.Contains(query.WindowsAccount));
            if (query.FromUtc.HasValue)
                q = q.Where(a => a.TimestampUtc >= query.FromUtc.Value);
            if (query.ToUtc.HasValue)
                q = q.Where(a => a.TimestampUtc <= query.ToUtc.Value);
            if (!string.IsNullOrWhiteSpace(query.Action))
                q = q.Where(a => a.Action == query.Action);
            if (!string.IsNullOrWhiteSpace(query.EntityType))
                q = q.Where(a => a.EntityType == query.EntityType);
            if (!string.IsNullOrWhiteSpace(query.EntityKey))
                q = q.Where(a => a.EntityKey == query.EntityKey);
            return q.OrderByDescending(a => a.TimestampUtc);
        }
    }
}
