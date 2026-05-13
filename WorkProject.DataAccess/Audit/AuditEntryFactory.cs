using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Newtonsoft.Json;
using WorkProject.Domain.Entities;
using WorkProject.Domain.Enums;

namespace WorkProject.DataAccess.Audit
{
    internal class PendingAuditEntry
    {
        public DbEntityEntry EntityEntry { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public Dictionary<string, object> BeforeValues { get; set; }
        public Dictionary<string, object> AfterValues { get; set; }
        public string PrimaryKeyName { get; set; }
    }

    internal static class AuditEntryFactory
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Include
        };

        public static List<PendingAuditEntry> Snapshot(DbContext context)
        {
            var pending = new List<PendingAuditEntry>();

            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                if (entry.Entity is AuditLog) continue;
                if (entry.State == EntityState.Unchanged || entry.State == EntityState.Detached) continue;

                var entityTypeName = entry.Entity.GetType().Name;
                var pendingEntry = new PendingAuditEntry
                {
                    EntityEntry = entry,
                    EntityType = entityTypeName,
                    PrimaryKeyName = DetectPrimaryKeyName(entityTypeName)
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        pendingEntry.Action = AuditAction.Insert;
                        pendingEntry.AfterValues = ToDictionary(entry.CurrentValues);
                        break;
                    case EntityState.Modified:
                        pendingEntry.Action = AuditAction.Update;
                        pendingEntry.BeforeValues = ToDictionary(entry.OriginalValues);
                        pendingEntry.AfterValues = ToDictionary(entry.CurrentValues);
                        break;
                    case EntityState.Deleted:
                        pendingEntry.Action = AuditAction.Delete;
                        pendingEntry.BeforeValues = ToDictionary(entry.OriginalValues);
                        break;
                }

                pending.Add(pendingEntry);
            }

            return pending;
        }

        public static List<AuditLog> BuildLogs(IEnumerable<PendingAuditEntry> pending, int? currentUserId, Guid correlationId)
        {
            var logs = new List<AuditLog>();
            var whenUtc = DateTime.UtcNow;

            foreach (var p in pending)
            {
                string entityId = null;
                if (!string.IsNullOrEmpty(p.PrimaryKeyName))
                {
                    var values = p.AfterValues ?? p.BeforeValues;
                    if (values != null && values.TryGetValue(p.PrimaryKeyName, out var pk) && pk != null)
                        entityId = pk.ToString();
                }

                logs.Add(new AuditLog
                {
                    Who_UserId = currentUserId,
                    WhenUtc = whenUtc,
                    Action = p.Action,
                    EntityType = p.EntityType,
                    EntityId = entityId,
                    BeforeJson = p.BeforeValues != null ? JsonConvert.SerializeObject(p.BeforeValues, JsonSettings) : null,
                    AfterJson = p.AfterValues != null ? JsonConvert.SerializeObject(p.AfterValues, JsonSettings) : null,
                    CorrelationId = correlationId
                });
            }

            return logs;
        }

        private static Dictionary<string, object> ToDictionary(DbPropertyValues values)
        {
            var dict = new Dictionary<string, object>();
            foreach (var name in values.PropertyNames)
            {
                var v = values[name];
                if (v is byte[]) continue;
                dict[name] = v;
            }
            return dict;
        }

        private static string DetectPrimaryKeyName(string entityTypeName)
        {
            switch (entityTypeName)
            {
                case "User": return "UserId";
                case "Project": return "ProjectId";
                case "ProjectMember": return "ProjectMemberId";
                case "ProjectTask": return "TaskId";
                case "TaskDependency": return "TaskDependencyId";
                case "Comment": return "CommentId";
                case "Attachment": return "AttachmentId";
                default: return null;
            }
        }
    }
}
