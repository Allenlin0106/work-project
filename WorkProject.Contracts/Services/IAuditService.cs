using WorkProject.Contracts.Dtos;

namespace WorkProject.Contracts.Services
{
    public interface IAuditService
    {
        PagedResult<AuditLogDto> Query(AuditQueryDto query);
        void RecordLogin(int? userId, string userName, bool success);
        void RecordLogout(int userId, string userName);
        void RecordExport(int userId, string what);
    }
}
