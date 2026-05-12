using System.Linq;
using ProjectPlanning.Entities.Dtos;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL.Interfaces
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        IQueryable<AuditLog> Search(AuditQuery query);
    }
}
