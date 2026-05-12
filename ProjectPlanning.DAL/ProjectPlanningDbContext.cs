using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ProjectPlanning.Common.Auditing;
using ProjectPlanning.DAL.Interfaces;
using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.DAL
{
    public class ProjectPlanningDbContext : DbContext, IUnitOfWork
    {
        private readonly ICurrentUserAccessor _currentUser;

        public ProjectPlanningDbContext()
            : base("name=ProjectPlanningDbContext")
        {
        }

        public ProjectPlanningDbContext(ICurrentUserAccessor currentUser)
            : base("name=ProjectPlanningDbContext")
        {
            _currentUser = currentUser;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskDependency> TaskDependencies { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<User>()
                .HasIndex(u => u.WindowsAccount).IsUnique();

            mb.Entity<Project>()
                .HasRequired(p => p.Owner).WithMany().HasForeignKey(p => p.OwnerId)
                .WillCascadeOnDelete(false);

            mb.Entity<TaskItem>()
                .HasRequired(t => t.Project).WithMany(p => p.Tasks).HasForeignKey(t => t.ProjectId)
                .WillCascadeOnDelete(false);
            mb.Entity<TaskItem>()
                .HasOptional(t => t.Assignee).WithMany().HasForeignKey(t => t.AssigneeId)
                .WillCascadeOnDelete(false);
            mb.Entity<TaskItem>()
                .HasOptional(t => t.ParentTask).WithMany(t => t.Children).HasForeignKey(t => t.ParentTaskId)
                .WillCascadeOnDelete(false);
            mb.Entity<TaskItem>()
                .HasIndex(t => new { t.ProjectId, t.SortOrder });

            mb.Entity<TaskDependency>()
                .HasRequired(d => d.PredecessorTask).WithMany(t => t.Successors).HasForeignKey(d => d.PredecessorTaskId)
                .WillCascadeOnDelete(false);
            mb.Entity<TaskDependency>()
                .HasRequired(d => d.SuccessorTask).WithMany(t => t.Predecessors).HasForeignKey(d => d.SuccessorTaskId)
                .WillCascadeOnDelete(false);
            mb.Entity<TaskDependency>()
                .HasIndex(d => new { d.PredecessorTaskId, d.SuccessorTaskId }).IsUnique();

            mb.Entity<ProjectMember>()
                .HasRequired(m => m.Project).WithMany(p => p.Members).HasForeignKey(m => m.ProjectId)
                .WillCascadeOnDelete(false);
            mb.Entity<ProjectMember>()
                .HasRequired(m => m.User).WithMany(u => u.Memberships).HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(false);
            mb.Entity<ProjectMember>()
                .HasIndex(m => new { m.ProjectId, m.UserId }).IsUnique();

            mb.Entity<Comment>()
                .HasRequired(c => c.Project).WithMany(p => p.Comments).HasForeignKey(c => c.ProjectId)
                .WillCascadeOnDelete(false);
            mb.Entity<Comment>()
                .HasOptional(c => c.Task).WithMany().HasForeignKey(c => c.TaskId)
                .WillCascadeOnDelete(false);
            mb.Entity<Comment>()
                .HasRequired(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId)
                .WillCascadeOnDelete(false);

            mb.Entity<Attachment>()
                .HasRequired(a => a.Project).WithMany(p => p.Attachments).HasForeignKey(a => a.ProjectId)
                .WillCascadeOnDelete(false);
            mb.Entity<Attachment>()
                .HasOptional(a => a.Task).WithMany().HasForeignKey(a => a.TaskId)
                .WillCascadeOnDelete(false);
            mb.Entity<Attachment>()
                .HasRequired(a => a.UploadedBy).WithMany().HasForeignKey(a => a.UploadedById)
                .WillCascadeOnDelete(false);

            mb.Entity<AuditLog>()
                .HasOptional(a => a.User).WithMany().HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);
            mb.Entity<AuditLog>().HasIndex(a => a.TimestampUtc);
            mb.Entity<AuditLog>().HasIndex(a => a.WindowsAccount);
            mb.Entity<AuditLog>().HasIndex(a => a.Action);
            mb.Entity<AuditLog>().HasIndex(a => a.EntityType);

            base.OnModelCreating(mb);
        }

        public override int SaveChanges()
        {
            var pendingAudits = BuildAuditEntries();
            var result = base.SaveChanges();
            if (pendingAudits.Count > 0)
            {
                FinalizeKeys(pendingAudits);
                WriteAuditLogs(pendingAudits);
                base.SaveChanges();
            }
            return result;
        }

        private List<PendingAudit> BuildAuditEntries()
        {
            var pendings = new List<PendingAudit>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog) continue;
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;

                var type = entry.Entity.GetType();
                if (type.Namespace != null && type.Namespace.StartsWith("System.Data.Entity.DynamicProxies"))
                {
                    type = type.BaseType;
                }
                var pending = new PendingAudit
                {
                    EntityType = type != null ? type.Name : entry.Entity.GetType().Name,
                    Entry = entry,
                    State = entry.State,
                    Snapshot = new EntitySnapshot()
                };

                var keyPropertyNames = GetKeyPropertyNames(entry);
                if (entry.State == EntityState.Added)
                {
                    foreach (var name in entry.CurrentValues.PropertyNames)
                    {
                        if (IsIgnoredProperty(name)) continue;
                        pending.Snapshot.After[name] = entry.CurrentValues[name];
                    }
                    pending.KeyPropertyNames = keyPropertyNames;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    foreach (var name in entry.OriginalValues.PropertyNames)
                    {
                        if (IsIgnoredProperty(name)) continue;
                        pending.Snapshot.Before[name] = entry.OriginalValues[name];
                    }
                    pending.EntityKey = BuildKey(entry, keyPropertyNames, useOriginal: true);
                }
                else if (entry.State == EntityState.Modified)
                {
                    foreach (var name in entry.OriginalValues.PropertyNames)
                    {
                        if (IsIgnoredProperty(name)) continue;
                        var prop = entry.Property(name);
                        if (prop.IsModified)
                        {
                            pending.Snapshot.Before[name] = entry.OriginalValues[name];
                            pending.Snapshot.After[name] = entry.CurrentValues[name];
                        }
                    }
                    pending.EntityKey = BuildKey(entry, keyPropertyNames, useOriginal: true);
                }
                pendings.Add(pending);
            }
            return pendings;
        }

        private static bool IsIgnoredProperty(string name)
        {
            return string.Equals(name, "RowVersion", StringComparison.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> GetKeyPropertyNames(DbEntityEntry entry)
        {
            try
            {
                var dbCtx = ((IObjectContextAdapter)entry.Context).ObjectContext;
                var os = dbCtx.ObjectStateManager.GetObjectStateEntry(entry.Entity);
                return os.EntityKey.EntityKeyValues.Select(k => k.Key).ToArray();
            }
            catch
            {
                return new[] { "Id" };
            }
        }

        private static string BuildKey(DbEntityEntry entry, IEnumerable<string> keyNames, bool useOriginal)
        {
            var values = new List<string>();
            foreach (var name in keyNames)
            {
                var val = useOriginal ? entry.OriginalValues[name] : entry.CurrentValues[name];
                values.Add(val == null ? "" : val.ToString());
            }
            return string.Join("|", values);
        }

        private void FinalizeKeys(List<PendingAudit> pendings)
        {
            foreach (var p in pendings)
            {
                if (string.IsNullOrEmpty(p.EntityKey) && p.KeyPropertyNames != null)
                {
                    p.EntityKey = BuildKey(p.Entry, p.KeyPropertyNames, useOriginal: false);
                    foreach (var name in p.KeyPropertyNames)
                    {
                        p.Snapshot.After[name] = p.Entry.CurrentValues[name];
                    }
                }
            }
        }

        private void WriteAuditLogs(List<PendingAudit> pendings)
        {
            var now = DateTime.UtcNow;
            foreach (var p in pendings)
            {
                string action;
                switch (p.State)
                {
                    case EntityState.Added: action = AuditAction.Create; break;
                    case EntityState.Modified: action = AuditAction.Update; break;
                    case EntityState.Deleted: action = AuditAction.Delete; break;
                    default: continue;
                }

                var log = new AuditLog
                {
                    TimestampUtc = now,
                    WindowsAccount = _currentUser != null ? _currentUser.WindowsAccount ?? "system" : "system",
                    UserId = _currentUser != null ? _currentUser.UserId : null,
                    Action = action,
                    EntityType = p.EntityType,
                    EntityKey = p.EntityKey,
                    BeforeJson = p.Snapshot.Before.Count == 0 ? null : AuditJsonSettings.Serialize(p.Snapshot.Before),
                    AfterJson = p.Snapshot.After.Count == 0 ? null : AuditJsonSettings.Serialize(p.Snapshot.After),
                    IpAddress = _currentUser != null ? _currentUser.IpAddress : null,
                    UserAgent = _currentUser != null ? _currentUser.UserAgent : null,
                    ControllerAction = _currentUser != null ? _currentUser.ControllerAction : null
                };
                AuditLogs.Add(log);
            }
        }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        private class EntitySnapshot
        {
            public Dictionary<string, object> Before { get; } = new Dictionary<string, object>();
            public Dictionary<string, object> After { get; } = new Dictionary<string, object>();
        }

        private class PendingAudit
        {
            public string EntityType { get; set; }
            public string EntityKey { get; set; }
            public EntityState State { get; set; }
            public DbEntityEntry Entry { get; set; }
            public EntitySnapshot Snapshot { get; set; }
            public IEnumerable<string> KeyPropertyNames { get; set; }
        }
    }
}
