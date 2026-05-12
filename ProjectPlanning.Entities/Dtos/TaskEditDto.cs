using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectPlanning.Entities.Dtos
{
    public class TaskEditDto
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(0, 100)]
        public byte ProgressPercent { get; set; }

        public int? AssigneeId { get; set; }

        public int? ParentTaskId { get; set; }

        public int SortOrder { get; set; }
    }
}
