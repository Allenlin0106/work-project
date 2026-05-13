using System;

namespace WorkProject.Contracts.Dtos
{
    public class NewTaskDto
    {
        public int ProjectId { get; set; }
        public int? ParentTaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AssignedUserId { get; set; }
        public bool IsMilestone { get; set; }
    }
}
