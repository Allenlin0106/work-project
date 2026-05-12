using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Models
{
    public class TaskDependency
    {
        public int Id { get; set; }

        public int PredecessorTaskId { get; set; }
        public virtual TaskItem PredecessorTask { get; set; }

        public int SuccessorTaskId { get; set; }
        public virtual TaskItem SuccessorTask { get; set; }

        public DependencyType Type { get; set; }
    }
}
