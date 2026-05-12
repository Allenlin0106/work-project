using System;

namespace ProjectPlanning.Entities.Dtos
{
    public class AuditQuery
    {
        public string WindowsAccount { get; set; }
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityKey { get; set; }
    }
}
