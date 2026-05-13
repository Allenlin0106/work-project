using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class ProjectTaskConfiguration : EntityTypeConfiguration<ProjectTask>
    {
        public ProjectTaskConfiguration()
        {
            ToTable("Tasks");
            HasKey(x => x.TaskId);
            Property(x => x.ProjectId).IsRequired();
            Property(x => x.Title).IsRequired().HasMaxLength(200);
            Property(x => x.Description).IsOptional();
            Property(x => x.StartDate).IsRequired();
            Property(x => x.EndDate).IsRequired();
            Property(x => x.ProgressPercent).IsRequired();
            Property(x => x.Status).IsRequired();
            Property(x => x.IsMilestone).IsRequired();
            Property(x => x.SortOrder).IsRequired();
            Property(x => x.CreatedUtc).IsRequired();
            Property(x => x.UpdatedUtc).IsRequired();
            Property(x => x.RowVersion).IsRowVersion();

            HasRequired(x => x.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.ProjectId)
                .WillCascadeOnDelete(true);

            HasOptional(x => x.ParentTask)
                .WithMany(p => p.SubTasks)
                .HasForeignKey(x => x.ParentTaskId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.AssignedUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
