using System;
using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Dtos
{
    public class ProjectMemberDto
    {
        public int ProjectMemberId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public ProjectRole Role { get; set; }
        public DateTime JoinedUtc { get; set; }
    }
}
