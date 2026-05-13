using WorkProject.Contracts.Dtos;
using WorkProject.Domain.Entities;

namespace WorkProject.Contracts.Repositories
{
    public interface IAuditRepository
    {
        void Add(AuditLog log);
        PagedResult<AuditLog> Query(AuditQueryDto query);
    }
}
