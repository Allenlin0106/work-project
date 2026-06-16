using System;
using System.Collections.Generic;

namespace ProjectPlanning.Entities.Dtos
{
    public class GanttDataDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
        public List<TaskDependencyDto> Dependencies { get; set; } = new List<TaskDependencyDto>();
    }
}
