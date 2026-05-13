using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class TaskDependencyConfiguration : EntityTypeConfiguration<TaskDependency>
    {
        public TaskDependencyConfiguration()
        {
            ToTable("TaskDependencies");
            HasKey(x => x.TaskDependencyId);
            Property(x => x.PredecessorTaskId).IsRequired();
            Property(x => x.SuccessorTaskId).IsRequired();
            Property(x => x.DependencyType).IsRequired();
            Property(x => x.LagDays).IsRequired();

            HasRequired(x => x.Predecessor)
                .WithMany(t => t.Successors)
                .HasForeignKey(x => x.PredecessorTaskId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Successor)
                .WithMany(t => t.Predecessors)
                .HasForeignKey(x => x.SuccessorTaskId)
                .WillCascadeOnDelete(false);
        }
    }
}
