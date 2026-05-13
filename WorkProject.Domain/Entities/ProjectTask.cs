using System;
using System.Collections.Generic;
using WorkProject.Domain.Enums;

namespace WorkProject.Domain.Entities
{
    public class ProjectTask
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
        public TaskStatus Status { get; set; }
        public bool IsMilestone { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProjectTask ParentTask { get; set; }
        public virtual User AssignedUser { get; set; }
        public virtual ICollection<ProjectTask> SubTasks { get; set; } = new List<ProjectTask>();
        public virtual ICollection<TaskDependency> Predecessors { get; set; } = new List<TaskDependency>();
        public virtual ICollection<TaskDependency> Successors { get; set; } = new List<TaskDependency>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
