using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class ProjectMemberConfiguration : EntityTypeConfiguration<ProjectMember>
    {
        public ProjectMemberConfiguration()
        {
            ToTable("ProjectMembers");
            HasKey(x => x.ProjectMemberId);
            Property(x => x.ProjectId).IsRequired();
            Property(x => x.UserId).IsRequired();
            Property(x => x.Role).IsRequired();
            Property(x => x.JoinedUtc).IsRequired();

            HasRequired(x => x.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(x => x.ProjectId)
                .WillCascadeOnDelete(true);

            HasRequired(x => x.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
