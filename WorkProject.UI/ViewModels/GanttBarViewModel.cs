using System;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;

namespace WorkProject.UI.ViewModels
{
    public class GanttBarViewModel : ViewModelBase
    {
        private readonly TaskDto _model;
        private readonly ITaskService _taskService;

        public GanttBarViewModel(TaskDto model, ITaskService taskService, int rowIndex)
        {
            _model = model;
            _taskService = taskService;
            RowIndex = rowIndex;
        }

        public int TaskId => _model.TaskId;
        public string Title => _model.Title;
        public int RowIndex { get; }

        public DateTime StartDate
        {
            get => _model.StartDate;
            set
            {
                if (_model.StartDate != value)
                {
                    _model.StartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EndDate
        {
            get => _model.EndDate;
            set
            {
                if (_model.EndDate != value)
                {
                    _model.EndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ProgressPercent
        {
            get => _model.ProgressPercent;
            set
            {
                if (_model.ProgressPercent != value)
                {
                    _model.ProgressPercent = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMilestone => _model.IsMilestone;

        public void CommitChanges()
        {
            _taskService.UpdateTask(_model);
        }
    }
}
