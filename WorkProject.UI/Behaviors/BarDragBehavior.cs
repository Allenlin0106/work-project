using System;
using System.Windows;
using System.Windows.Input;
using WorkProject.UI.Controls.Gantt;
using WorkProject.UI.ViewModels;

namespace WorkProject.UI.Behaviors
{
    public static class BarDragBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(BarDragBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(DependencyObject d, bool value) => d.SetValue(IsEnabledProperty, value);
        public static bool GetIsEnabled(DependencyObject d) => (bool)d.GetValue(IsEnabledProperty);

        private static Point _dragStart;
        private static bool _dragging;
        private static GanttBarViewModel _draggingVm;
        private static DateTime _originalStart;
        private static DateTime _originalEnd;

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement fe)) return;
            if ((bool)e.NewValue)
            {
                fe.MouseLeftButtonDown += OnDown;
                fe.MouseMove += OnMove;
                fe.MouseLeftButtonUp += OnUp;
            }
            else
            {
                fe.MouseLeftButtonDown -= OnDown;
                fe.MouseMove -= OnMove;
                fe.MouseLeftButtonUp -= OnUp;
            }
        }

        private static void OnDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement fe)) return;
            if (!(fe.DataContext is GanttBarViewModel vm)) return;
            _dragStart = e.GetPosition(null);
            _draggingVm = vm;
            _originalStart = vm.StartDate;
            _originalEnd = vm.EndDate;
            _dragging = true;
            fe.CaptureMouse();
        }

        private static void OnMove(object sender, MouseEventArgs e)
        {
            if (!_dragging || _draggingVm == null) return;
            if (!(sender is FrameworkElement fe)) return;

            var current = e.GetPosition(null);
            var deltaPixels = current.X - _dragStart.X;
            var pxPerDay = GanttCanvas.GetPixelsPerDay(fe);
            if (pxPerDay <= 0) return;
            var deltaDays = (int)Math.Round(deltaPixels / pxPerDay);
            _draggingVm.StartDate = _originalStart.AddDays(deltaDays);
            _draggingVm.EndDate = _originalEnd.AddDays(deltaDays);
        }

        private static void OnUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement fe)) return;
            fe.ReleaseMouseCapture();
            if (_dragging && _draggingVm != null)
            {
                _draggingVm.CommitChanges();
            }
            _dragging = false;
            _draggingVm = null;
        }
    }
}
