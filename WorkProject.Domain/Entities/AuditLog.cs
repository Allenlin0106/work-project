using System;

namespace WorkProject.Domain.Entities
{
    public class AuditLog
    {
        public long AuditLogId { get; set; }
        public int? Who_UserId { get; set; }
        public DateTime WhenUtc { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string BeforeJson { get; set; }
        public string AfterJson { get; set; }
        public Guid CorrelationId { get; set; }

        public virtual User Who { get; set; }
    }
}
