using System;
using ProjectPlanning.Entities.Dtos;

namespace ProjectPlanning.Web.Models
{
    public class AuditLogFilterVM
    {
        public string WindowsAccount { get; set; }
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityKey { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public PagedResult<AuditLogDto> Result { get; set; }
    }
}
