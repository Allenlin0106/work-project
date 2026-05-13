using WorkProject.Domain.Enums;

namespace WorkProject.Contracts.Dtos
{
    public class TaskDependencyDto
    {
        public int TaskDependencyId { get; set; }
        public int PredecessorTaskId { get; set; }
        public int SuccessorTaskId { get; set; }
        public DependencyType DependencyType { get; set; }
        public int LagDays { get; set; }
    }
}
