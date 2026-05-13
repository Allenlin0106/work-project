using System;
using WorkProject.Domain.Enums;

namespace WorkProject.Domain.Entities
{
    public class ProjectMember
    {
        public int ProjectMemberId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public ProjectRole Role { get; set; }
        public DateTime JoinedUtc { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
