using ProjectPlanning.Entities.Enums;

namespace ProjectPlanning.Entities.Dtos
{
    public class TaskDependencyDto
    {
        public int Id { get; set; }
        public int PredecessorTaskId { get; set; }
        public int SuccessorTaskId { get; set; }
        public DependencyType Type { get; set; }
    }
}
