using System;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Dtos
{
    public class ProjectListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public string OwnerDisplayName { get; set; }
        public int TaskCount { get; set; }
        public int MemberCount { get; set; }
    }
}
