using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unity;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;
using WorkProject.UI.Helpers;

namespace WorkProject.UI.ViewModels
{
    public class ProjectDetailViewModel : ViewModelBase
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly IExportService _exportService;
        private readonly IUnityContainer _container;
        private ProjectDto _project;
        private ProjectGanttViewModel _ganttViewModel;
        private TaskDto _selectedTask;

        public ProjectDetailViewModel(IProjectService projectService, ITaskService taskService,
            IExportService exportService, IUnityContainer container)
        {
            _projectService = projectService;
            _taskService = taskService;
            _exportService = exportService;
            _container = container;
            Tasks = new ObservableCollection<TaskDto>();

            AddTaskCommand = new RelayCommand(_ => AddTask());
            EditTaskCommand = new RelayCommand(_ => EditSelected(), _ => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedTask != null);
            ExportPdfCommand = new RelayCommand(snapshotElement => ExportPdf(snapshotElement));
            RefreshCommand = new RelayCommand(_ => Refresh());
        }

        public ProjectDto Project
        {
            get => _project;
            set => SetField(ref _project, value);
        }

        public ProjectGanttViewModel GanttViewModel
        {
            get => _ganttViewModel;
            set => SetField(ref _ganttViewModel, value);
        }

        public ObservableCollection<TaskDto> Tasks { get; }

        public TaskDto SelectedTask
        {
            get => _selectedTask;
            set => SetField(ref _selectedTask, value);
        }

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand ExportPdfCommand { get; }
        public ICommand RefreshCommand { get; }

        public void Load(int projectId)
        {
            Project = _projectService.Get(projectId);
            Refresh();
        }

        private void Refresh()
        {
            if (Project == null) return;
            Tasks.Clear();
            var list = _taskService.ListByProject(Project.ProjectId);
            foreach (var t in list) Tasks.Add(t);

            var ganttVm = _container.Resolve<ProjectGanttViewModel>();
            ganttVm.Load(Project, list);
            GanttViewModel = ganttVm;
        }

        private void AddTask()
        {
            var vm = _container.Resolve<TaskEditViewModel>();
            vm.LoadForNew(Project.ProjectId);
            var dlg = new Views.TaskEditDialog { DataContext = vm };
            vm.CloseAction = ok => { dlg.DialogResult = ok; dlg.Close(); };
            if (dlg.ShowDialog() == true) Refresh();
        }

        private void EditSelected()
        {
            if (SelectedTask == null) return;
            var vm = _container.Resolve<TaskEditViewModel>();
            vm.LoadForEdit(SelectedTask);
            var dlg = new Views.TaskEditDialog { DataContext = vm };
            vm.CloseAction = ok => { dlg.DialogResult = ok; dlg.Close(); };
            if (dlg.ShowDialog() == true) Refresh();
        }

        private void DeleteSelected()
        {
            if (SelectedTask == null) return;
            if (!DialogService.Confirm("Delete task", $"Delete task '{SelectedTask.Title}'?")) return;
            try
            {
                _taskService.DeleteTask(SelectedTask.TaskId);
                Refresh();
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Delete task", ex.Message);
            }
        }

        private void ExportPdf(object snapshotElement)
        {
            if (Project == null) return;
            var path = DialogService.ShowSavePdfDialog($"{SanitizeFileName(Project.Name)}.pdf");
            if (string.IsNullOrEmpty(path)) return;

            byte[] ganttImage = null;
            if (snapshotElement is FrameworkElement fe)
                ganttImage = GanttSnapshotRenderer.RenderToPng(fe);

            try
            {
                _exportService.ExportProjectToFile(Project.ProjectId, path, new ExportOptions
                {
                    IncludeGantt = ganttImage != null,
                    GanttImage = ganttImage
                });
                DialogService.ShowInfo("Export", $"Exported to {path}");
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Export", ex.Message);
            }
        }

        private static string SanitizeFileName(string s)
        {
            foreach (var c in Path.GetInvalidFileNameChars()) s = s.Replace(c, '_');
            return s;
        }
    }
}
