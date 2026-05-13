using System.Windows;
using WorkProject.UI.ViewModels;

namespace WorkProject.UI.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.CloseAction = () =>
            {
                DialogResult = _viewModel.LoginSucceeded;
                Close();
            };
        }
    }
}
