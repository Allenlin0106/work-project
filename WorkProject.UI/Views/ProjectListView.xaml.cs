using System.Windows.Controls;
using System.Windows.Input;
using WorkProject.UI.ViewModels;

namespace WorkProject.UI.Views
{
    public partial class ProjectListView : UserControl
    {
        public ProjectListView()
        {
            InitializeComponent();
        }

        private void OnProjectDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ProjectListViewModel vm && vm.OpenProjectCommand.CanExecute(null))
                vm.OpenProjectCommand.Execute(null);
        }
    }
}
