using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanning.Entities.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public byte ProgressPercent { get; set; }

        public int? AssigneeId { get; set; }
        public virtual User Assignee { get; set; }

        public int? ParentTaskId { get; set; }
        public virtual TaskItem ParentTask { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<TaskItem> Children { get; set; }
        public virtual ICollection<TaskDependency> Predecessors { get; set; }
        public virtual ICollection<TaskDependency> Successors { get; set; }
    }
}
