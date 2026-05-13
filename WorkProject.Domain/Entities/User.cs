using System;
using System.Collections.Generic;

namespace WorkProject.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int PasswordIterations { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedUtc { get; set; }

        public virtual ICollection<ProjectMember> Memberships { get; set; } = new List<ProjectMember>();
        public virtual ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    }
}
