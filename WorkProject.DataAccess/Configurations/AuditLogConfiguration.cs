using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class AuditLogConfiguration : EntityTypeConfiguration<AuditLog>
    {
        public AuditLogConfiguration()
        {
            ToTable("AuditLogs");
            HasKey(x => x.AuditLogId);
            Property(x => x.WhenUtc).IsRequired();
            Property(x => x.Action).IsRequired().HasMaxLength(16);
            Property(x => x.EntityType).IsRequired().HasMaxLength(64);
            Property(x => x.EntityId).HasMaxLength(64);
            Property(x => x.BeforeJson).IsOptional();
            Property(x => x.AfterJson).IsOptional();
            Property(x => x.CorrelationId).IsRequired();

            HasOptional(x => x.Who)
                .WithMany()
                .HasForeignKey(x => x.Who_UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
