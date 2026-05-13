using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            ToTable("Projects");
            HasKey(x => x.ProjectId);
            Property(x => x.Name).IsRequired().HasMaxLength(200);
            Property(x => x.Description).IsOptional();
            Property(x => x.StartDate).IsRequired();
            Property(x => x.EndDate).IsRequired();
            Property(x => x.Status).IsRequired();
            Property(x => x.OwnerUserId).IsRequired();
            Property(x => x.CreatedUtc).IsRequired();
            Property(x => x.UpdatedUtc).IsRequired();
            Property(x => x.RowVersion).IsRowVersion();

            HasRequired(x => x.Owner)
                .WithMany(u => u.OwnedProjects)
                .HasForeignKey(x => x.OwnerUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
