using System;
using System.Collections.Generic;
using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Dtos
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public int OwnerUserId { get; set; }
        public string OwnerDisplayName { get; set; }
        public List<ProjectMemberDto> Members { get; set; } = new List<ProjectMemberDto>();
    }
}
