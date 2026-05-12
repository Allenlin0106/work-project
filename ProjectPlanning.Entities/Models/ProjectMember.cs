using System;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public ProjectRole Role { get; set; }

        public DateTime AddedUtc { get; set; }
    }
}
