using System.Windows.Input;
using Unity;
using WorkProject.Contracts.Services;
using WorkProject.UI.Helpers;

namespace WorkProject.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private readonly IAuthService _authService;
        private object _currentView;

        public MainViewModel(IUnityContainer container, IAuthService authService)
        {
            _container = container;
            _authService = authService;
            ShowProjectsCommand = new RelayCommand(_ => ShowProjects());
            ShowAuditCommand = new RelayCommand(_ => ShowAudit());
            LogoutCommand = new RelayCommand(_ => Logout());

            ShowProjects();
        }

        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public string CurrentUserName => _authService.CurrentUser?.DisplayName ?? "(guest)";

        public ICommand ShowProjectsCommand { get; }
        public ICommand ShowAuditCommand { get; }
        public ICommand LogoutCommand { get; }

        public System.Action LogoutAction { get; set; }

        private void ShowProjects()
        {
            CurrentView = _container.Resolve<ProjectListViewModel>();
        }

        private void ShowAudit()
        {
            CurrentView = _container.Resolve<AuditLogViewModel>();
        }

        private void Logout()
        {
            _authService.Logout();
            LogoutAction?.Invoke();
        }
    }
}
