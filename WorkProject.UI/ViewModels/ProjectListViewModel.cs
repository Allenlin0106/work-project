using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;
using WorkProject.Contracts.Dtos;
using WorkProject.Contracts.Services;
using WorkProject.UI.Helpers;

namespace WorkProject.UI.ViewModels
{
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly IProjectService _projectService;
        private readonly IUnityContainer _container;
        private ProjectSummaryDto _selectedProject;
        private object _detailView;
        private string _newProjectName;
        private DateTime _newProjectStart = DateTime.Today;
        private DateTime _newProjectEnd = DateTime.Today.AddDays(30);

        public ProjectListViewModel(IProjectService projectService, IUnityContainer container)
        {
            _projectService = projectService;
            _container = container;
            Projects = new ObservableCollection<ProjectSummaryDto>();
            RefreshCommand = new RelayCommand(_ => Refresh());
            OpenProjectCommand = new RelayCommand(_ => OpenSelected(), _ => SelectedProject != null);
            CreateProjectCommand = new RelayCommand(_ => CreateProject(), _ => !string.IsNullOrWhiteSpace(NewProjectName));
            DeleteProjectCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedProject != null);
            Refresh();
        }

        public ObservableCollection<ProjectSummaryDto> Projects { get; }

        public ProjectSummaryDto SelectedProject
        {
            get => _selectedProject;
            set => SetField(ref _selectedProject, value);
        }

        public object DetailView
        {
            get => _detailView;
            set => SetField(ref _detailView, value);
        }

        public string NewProjectName
        {
            get => _newProjectName;
            set => SetField(ref _newProjectName, value);
        }

        public DateTime NewProjectStart
        {
            get => _newProjectStart;
            set => SetField(ref _newProjectStart, value);
        }

        public DateTime NewProjectEnd
        {
            get => _newProjectEnd;
            set => SetField(ref _newProjectEnd, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand CreateProjectCommand { get; }
        public ICommand DeleteProjectCommand { get; }

        private void Refresh()
        {
            Projects.Clear();
            foreach (var p in _projectService.ListForCurrentUser())
                Projects.Add(p);
        }

        private void OpenSelected()
        {
            if (SelectedProject == null) return;
            var detail = _container.Resolve<ProjectDetailViewModel>();
            detail.Load(SelectedProject.ProjectId);
            DetailView = detail;
        }

        private void CreateProject()
        {
            try
            {
                _projectService.CreateProject(new NewProjectDto
                {
                    Name = NewProjectName,
                    StartDate = NewProjectStart,
                    EndDate = NewProjectEnd
                });
                NewProjectName = string.Empty;
                Refresh();
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Create project", ex.Message);
            }
        }

        private void DeleteSelected()
        {
            if (SelectedProject == null) return;
            if (!DialogService.Confirm("Delete project",
                $"Are you sure you want to delete '{SelectedProject.Name}'? Tasks and members will be removed.")) return;
            try
            {
                _projectService.DeleteProject(SelectedProject.ProjectId);
                DetailView = null;
                Refresh();
            }
            catch (Exception ex)
            {
                DialogService.ShowError("Delete project", ex.Message);
            }
        }
    }
}
