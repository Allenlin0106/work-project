using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            ToTable("Comments");
            HasKey(x => x.CommentId);
            Property(x => x.Body).IsRequired();
            Property(x => x.AuthorUserId).IsRequired();
            Property(x => x.CreatedUtc).IsRequired();

            HasOptional(x => x.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(x => x.TaskId)
                .WillCascadeOnDelete(true);

            HasOptional(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Author)
                .WithMany()
                .HasForeignKey(x => x.AuthorUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
