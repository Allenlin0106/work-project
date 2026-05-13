using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using WorkProject.Contracts.Services;
using WorkProject.DataAccess.Audit;
using WorkProject.DataAccess.Configurations;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Context
{
    public class WorkProjectDbContext : DbContext
    {
        private readonly ICurrentUserAccessor _currentUser;
        private int? _overrideUserId;

        public WorkProjectDbContext()
            : base("name=WorkProjectDb")
        {
            _currentUser = null;
        }

        public WorkProjectDbContext(ICurrentUserAccessor currentUser)
            : base("name=WorkProjectDb")
        {
            _currentUser = currentUser;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<TaskDependency> TaskDependencies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public void SetCurrentUser(int? userId)
        {
            _overrideUserId = userId;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new ProjectMemberConfiguration());
            modelBuilder.Configurations.Add(new ProjectTaskConfiguration());
            modelBuilder.Configurations.Add(new TaskDependencyConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new AttachmentConfiguration());
            modelBuilder.Configurations.Add(new AuditLogConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var pending = AuditEntryFactory.Snapshot(this);
            if (pending.Count == 0)
            {
                return base.SaveChanges();
            }

            var correlationId = Guid.NewGuid();
            var who = _overrideUserId ?? _currentUser?.CurrentUserId;
            var now = DateTime.UtcNow;
            foreach (var p in pending)
            {
                if (p.AfterValues != null)
                {
                    if (p.AfterValues.ContainsKey("CreatedUtc") && p.Action == Domain.Enums.AuditAction.Insert)
                    {
                        p.AfterValues["CreatedUtc"] = now;
                    }
                    if (p.AfterValues.ContainsKey("UpdatedUtc"))
                    {
                        p.AfterValues["UpdatedUtc"] = now;
                    }
                }
            }

            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var affected = base.SaveChanges();

                var refreshed = AuditEntryFactory.Snapshot(this);
                foreach (var p in pending.Where(x => x.Action == Domain.Enums.AuditAction.Insert))
                {
                    if (p.AfterValues == null) continue;
                    var current = p.EntityEntry.CurrentValues;
                    foreach (var name in current.PropertyNames)
                    {
                        if (p.AfterValues.ContainsKey(name))
                        {
                            var v = current[name];
                            if (!(v is byte[])) p.AfterValues[name] = v;
                        }
                    }
                }

                var logs = AuditEntryFactory.BuildLogs(pending, who, correlationId);
                foreach (var log in logs)
                {
                    AuditLogs.Add(log);
                }
                base.SaveChanges();

                scope.Complete();
                return affected;
            }
        }
    }
}
