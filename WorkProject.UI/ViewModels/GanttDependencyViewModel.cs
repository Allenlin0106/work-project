using WorkProject.Contracts.Dtos;

namespace WorkProject.UI.ViewModels
{
    public class GanttDependencyViewModel : ViewModelBase
    {
        public GanttDependencyViewModel(TaskDependencyDto model)
        {
            DependencyId = model.TaskDependencyId;
            PredecessorTaskId = model.PredecessorTaskId;
            SuccessorTaskId = model.SuccessorTaskId;
            LagDays = model.LagDays;
        }

        public int DependencyId { get; }
        public int PredecessorTaskId { get; }
        public int SuccessorTaskId { get; }
        public int LagDays { get; }
    }
}
