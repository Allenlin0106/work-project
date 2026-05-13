using Microsoft.Win32;
using System.Windows;

namespace WorkProject.UI.Helpers
{
    public static class DialogService
    {
        public static string ShowSavePdfDialog(string defaultFileName)
        {
            var dlg = new SaveFileDialog
            {
                FileName = defaultFileName,
                DefaultExt = ".pdf",
                Filter = "PDF documents (*.pdf)|*.pdf"
            };
            return dlg.ShowDialog() == true ? dlg.FileName : null;
        }

        public static void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowInfo(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool Confirm(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes;
        }
    }
}
