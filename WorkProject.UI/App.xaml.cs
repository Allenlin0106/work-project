using System;
using System.Windows;
using Unity;
using WorkProject.Contracts.Services;
using WorkProject.UI.Composition;
using WorkProject.UI.Views;

namespace WorkProject.UI
{
    public partial class App : Application
    {
        private IUnityContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _container = UnityBootstrapper.Configure();

            try
            {
                _container.Resolve<IDatabaseInitializer>().Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialise database: " + ex.Message,
                    "Startup error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            var login = _container.Resolve<LoginWindow>();
            var ok = login.ShowDialog();
            if (ok != true)
            {
                Shutdown();
                return;
            }

            var main = _container.Resolve<MainWindow>();
            MainWindow = main;
            main.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container?.Dispose();
            base.OnExit(e);
        }
    }
}
