using System;
using System.Windows.Input;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;
using WorkProject.UI.Helpers;

namespace WorkProject.UI.ViewModels
{
    public class TaskEditViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private bool _isNew;
        private int _taskId;
        private int _projectId;
        private string _title;
        private string _description;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today.AddDays(5);
        private int _progressPercent;
        private bool _isMilestone;
        private Domain.Enums.TaskStatus _status;

        public TaskEditViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => CloseAction?.Invoke(false));
        }

        public string Title { get => _title; set => SetField(ref _title, value); }
        public string Description { get => _description; set => SetField(ref _description, value); }
        public DateTime StartDate { get => _startDate; set => SetField(ref _startDate, value); }
        public DateTime EndDate { get => _endDate; set => SetField(ref _endDate, value); }
        public int ProgressPercent { get => _progressPercent; set => SetField(ref _progressPercent, value); }
        public bool IsMilestone { get => _isMilestone; set => SetField(ref _isMilestone, value); }
        public Domain.Enums.TaskStatus Status { get => _status; set => SetField(ref _status, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action<bool> CloseAction { get; set; }

        public void LoadForNew(int projectId)
        {
            _isNew = true;
            _projectId = projectId;
            Title = string.Empty;
            Description = string.Empty;
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(5);
            ProgressPercent = 0;
            IsMilestone = false;
            Status = Domain.Enums.TaskStatus.NotStarted;
        }

        public void LoadForEdit(TaskDto task)
        {
            _isNew = false;
            _taskId = task.TaskId;
            _projectId = task.ProjectId;
            Title = task.Title;
            Description = task.Description;
            StartDate = task.StartDate;
            EndDate = task.EndDate;
            ProgressPercent = task.ProgressPercent;
            IsMilestone = task.IsMilestone;
            Status = task.Status;
        }

        private void Save()
        {
            try
            {
                if (_isNew)
                {
                    _taskService.CreateTask(new NewTaskDto
                    {
                        ProjectId = _projectId,
                        Title = Title,
                        Description = Description,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        IsMilestone = IsMilestone
                    });
                }
                else
                {
                    _taskService.UpdateTask(new TaskDto
                    {
                        TaskId = _taskId,
                        ProjectId = _projectId,
                        Title = Title,
                        Description = Description,
                        StartDate = StartDate,
                        EndDate = EndDate,
                        ProgressPercent = ProgressPercent,
                        IsMilestone = IsMilestone,
                        Status = Status
                    });
                }
                CloseAction?.Invoke(true);
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Save task", ex.Message);
            }
        }
    }
}
