using System.Collections.Generic;

namespace ProjectPlanning.Common.Auditing
{
    public class AuditEntry
    {
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityKey { get; set; }
        public Dictionary<string, object> Before { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> After { get; set; } = new Dictionary<string, object>();
        public List<KeyPlaceholder> TempKeyProperties { get; set; } = new List<KeyPlaceholder>();
    }

    public class KeyPlaceholder
    {
        public string PropertyName { get; set; }
        public object Owner { get; set; }
    }
}
