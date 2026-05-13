using System;
using System.Collections.Generic;
using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Dtos
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public int? ParentTaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProgressPercent { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedDisplayName { get; set; }
        public TaskStatus Status { get; set; }
        public bool IsMilestone { get; set; }
        public int SortOrder { get; set; }
        public List<TaskDependencyDto> Dependencies { get; set; } = new List<TaskDependencyDto>();
    }
}
