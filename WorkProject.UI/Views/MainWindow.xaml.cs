using System.Windows;
using WorkProject.UI.ViewModels;

namespace WorkProject.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.LogoutAction = () => { Application.Current.Shutdown(); };
        }
    }
}
