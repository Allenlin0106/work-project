using System;
using System.ComponentModel.DataAnnotations;
using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Dtos
{
    public class ProjectEditDto
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ProjectStatus Status { get; set; }
    }
}
