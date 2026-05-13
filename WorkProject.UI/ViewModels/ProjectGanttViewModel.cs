using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;
using WorkProject.UI.Controls.Gantt;

namespace WorkProject.UI.ViewModels
{
    public class ProjectGanttViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private DateTime _rangeStart;
        private DateTime _rangeEnd;
        private double _pixelsPerDay = 24.0;

        public ProjectGanttViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Bars = new ObservableCollection<GanttBarViewModel>();
            Dependencies = new ObservableCollection<GanttDependencyViewModel>();
            _rangeStart = DateTime.Today;
            _rangeEnd = DateTime.Today.AddMonths(3);
        }

        public ObservableCollection<GanttBarViewModel> Bars { get; }
        public ObservableCollection<GanttDependencyViewModel> Dependencies { get; }

        public DateTime RangeStart
        {
            get => _rangeStart;
            set { if (SetField(ref _rangeStart, value)) OnPropertyChanged(nameof(TotalWidth)); }
        }

        public DateTime RangeEnd
        {
            get => _rangeEnd;
            set { if (SetField(ref _rangeEnd, value)) OnPropertyChanged(nameof(TotalWidth)); }
        }

        public double PixelsPerDay
        {
            get => _pixelsPerDay;
            set { if (SetField(ref _pixelsPerDay, value)) OnPropertyChanged(nameof(TotalWidth)); }
        }

        public double TotalWidth => Math.Max(1, (RangeEnd.Date - RangeStart.Date).TotalDays) * PixelsPerDay;

        public double TotalHeight => Math.Max(1, Bars.Count) * GanttCanvas.RowHeight;

        public void Load(ProjectDto project, IReadOnlyList<TaskDto> tasks)
        {
            Bars.Clear();
            Dependencies.Clear();

            if (tasks.Count == 0)
            {
                RangeStart = project.StartDate.Date;
                RangeEnd = project.EndDate.Date.AddDays(1);
            }
            else
            {
                RangeStart = tasks.Min(t => t.StartDate).Date.AddDays(-3);
                RangeEnd = tasks.Max(t => t.EndDate).Date.AddDays(7);
            }

            int row = 0;
            foreach (var t in tasks.OrderBy(t => t.SortOrder))
            {
                Bars.Add(new GanttBarViewModel(t, _taskService, row++));
                foreach (var dep in t.Dependencies)
                    Dependencies.Add(new GanttDependencyViewModel(dep));
            }
            OnPropertyChanged(nameof(TotalHeight));
            OnPropertyChanged(nameof(TotalWidth));
        }
    }
}
