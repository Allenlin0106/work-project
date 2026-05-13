using System;
using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Dtos
{
    public class ProjectSummaryDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public double ProgressPercent { get; set; }
    }
}
