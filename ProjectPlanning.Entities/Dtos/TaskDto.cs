using System;

namespace ProjectPlanning.Entities.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte ProgressPercent { get; set; }
        public int? AssigneeId { get; set; }
        public string AssigneeDisplayName { get; set; }
        public int? ParentTaskId { get; set; }
        public int SortOrder { get; set; }
    }
}
