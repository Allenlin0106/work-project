using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanning.Entities.Models
{
    public class AuditLog
    {
        public long Id { get; set; }

        public DateTime TimestampUtc { get; set; }

        [Required, StringLength(256)]
        public string WindowsAccount { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [Required, StringLength(64)]
        public string Action { get; set; }

        [StringLength(128)]
        public string EntityType { get; set; }

        [StringLength(64)]
        public string EntityKey { get; set; }

        public string BeforeJson { get; set; }

        public string AfterJson { get; set; }

        [StringLength(64)]
        public string IpAddress { get; set; }

        [StringLength(400)]
        public string UserAgent { get; set; }

        [StringLength(200)]
        public string ControllerAction { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
