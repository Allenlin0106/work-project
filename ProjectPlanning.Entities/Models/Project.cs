using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ProjectStatus Status { get; set; }

        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }
        public virtual ICollection<ProjectMember> Members { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
