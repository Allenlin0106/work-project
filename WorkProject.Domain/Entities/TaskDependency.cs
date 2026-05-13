using WorkProject.Domain.Enums;

namespace WorkProject.Domain.Entities
{
    public class TaskDependency
    {
        public int TaskDependencyId { get; set; }
        public int PredecessorTaskId { get; set; }
        public int SuccessorTaskId { get; set; }
        public DependencyType DependencyType { get; set; }
        public int LagDays { get; set; }

        public virtual ProjectTask Predecessor { get; set; }
        public virtual ProjectTask Successor { get; set; }
    }
}
