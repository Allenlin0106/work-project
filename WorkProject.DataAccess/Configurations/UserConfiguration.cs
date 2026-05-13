using System.Data.Entity.ModelConfiguration;
using WorkProject.Domain.Entities;

namespace WorkProject.DataAccess.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(x => x.UserId);
            Property(x => x.UserName).IsRequired().HasMaxLength(64)
                .HasColumnAnnotation("Index",
                    new System.Data.Entity.Infrastructure.Annotations.IndexAnnotation(
                        new System.ComponentModel.DataAnnotations.Schema.IndexAttribute("IX_Users_UserName") { IsUnique = true }));
            Property(x => x.DisplayName).IsRequired().HasMaxLength(128);
            Property(x => x.PasswordHash).IsRequired().HasMaxLength(32);
            Property(x => x.PasswordSalt).IsRequired().HasMaxLength(16);
            Property(x => x.PasswordIterations).IsRequired();
            Property(x => x.IsActive).IsRequired();
            Property(x => x.CreatedUtc).IsRequired();
        }
    }
}
