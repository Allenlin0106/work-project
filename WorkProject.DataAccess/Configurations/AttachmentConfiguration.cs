using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class AttachmentConfiguration : EntityTypeConfiguration<Attachment>
    {
        public AttachmentConfiguration()
        {
            ToTable("Attachments");
            HasKey(x => x.AttachmentId);
            Property(x => x.FileName).IsRequired().HasMaxLength(260);
            Property(x => x.ContentType).IsRequired().HasMaxLength(128);
            Property(x => x.Content).IsRequired();
            Property(x => x.UploadedByUserId).IsRequired();
            Property(x => x.CreatedUtc).IsRequired();

            HasOptional(x => x.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(x => x.TaskId)
                .WillCascadeOnDelete(true);

            HasOptional(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.UploadedBy)
                .WithMany()
                .HasForeignKey(x => x.UploadedByUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
