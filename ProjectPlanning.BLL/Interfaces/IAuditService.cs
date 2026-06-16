using ProjectPlanning.Entities.Dtos;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface IAuditService
    {
        void LogLogin(string windowsAccount);
        void LogLogout(string windowsAccount);
        void LogView(string entityType, string entityKey, string notes = null);
        void LogExport(string entityType, string entityKey, string format);
        PagedResult<AuditLogDto> Search(AuditQuery query, int page, int pageSize);
    }
}
