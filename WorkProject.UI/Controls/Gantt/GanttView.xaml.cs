using System.Windows;
using System.Windows.Controls;

namespace WorkProject.UI.Controls.Gantt
{
    public partial class GanttView : UserControl
    {
        public GanttView()
        {
            InitializeComponent();
        }

        private void BarsScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (HeaderScroll != null && e.HorizontalChange != 0)
                HeaderScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
    }
}
