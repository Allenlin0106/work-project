using System.Windows.Input;
using WorkProject.Contracts.Services;
using WorkProject.UI.Helpers;

namespace WorkProject.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private string _userName;
        private string _errorMessage;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(o => ExecuteLogin(o), o => CanLogin(o));
        }

        public string UserName
        {
            get => _userName;
            set => SetField(ref _userName, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public bool LoginSucceeded { get; private set; }

        public ICommand LoginCommand { get; }

        public System.Action CloseAction { get; set; }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(UserName)
                   && parameter is System.Windows.Controls.PasswordBox pb
                   && !string.IsNullOrEmpty(pb.Password);
        }

        private void ExecuteLogin(object parameter)
        {
            var pb = parameter as System.Windows.Controls.PasswordBox;
            var result = _authService.Login(UserName, pb?.Password ?? string.Empty);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
                return;
            }
            LoginSucceeded = true;
            CloseAction?.Invoke();
        }
    }
}
