using System;
using System.Collections.Generic;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Dtos
{
    public class ProjectDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public int OwnerId { get; set; }
        public string OwnerDisplayName { get; set; }
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}
