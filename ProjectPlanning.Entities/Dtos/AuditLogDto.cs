using System;

namespace ProjectPlanning.Entities.Dtos
{
    public class AuditLogDto
    {
        public long Id { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string WindowsAccount { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityKey { get; set; }
        public string BeforeJson { get; set; }
        public string AfterJson { get; set; }
        public string IpAddress { get; set; }
        public string ControllerAction { get; set; }
        public string Notes { get; set; }
    }
}
