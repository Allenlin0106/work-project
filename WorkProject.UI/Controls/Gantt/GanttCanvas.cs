using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkProject.UI.Controls.Gantt
{
    public static class GanttCanvas
    {
        public static readonly DependencyProperty PixelsPerDayProperty =
            DependencyProperty.RegisterAttached(
                "PixelsPerDay",
                typeof(double),
                typeof(GanttCanvas),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static void SetPixelsPerDay(DependencyObject d, double value) => d.SetValue(PixelsPerDayProperty, value);
        public static double GetPixelsPerDay(DependencyObject d) => (double)d.GetValue(PixelsPerDayProperty);

        public static readonly DependencyProperty RangeStartProperty =
            DependencyProperty.RegisterAttached(
                "RangeStart",
                typeof(DateTime),
                typeof(GanttCanvas),
                new FrameworkPropertyMetadata(DateTime.Today,
                    FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsArrange));

        public static void SetRangeStart(DependencyObject d, DateTime value) => d.SetValue(RangeStartProperty, value);
        public static DateTime GetRangeStart(DependencyObject d) => (DateTime)d.GetValue(RangeStartProperty);

        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.RegisterAttached(
                "StartDate",
                typeof(DateTime),
                typeof(GanttCanvas),
                new PropertyMetadata(DateTime.Today, OnGeometryChanged));

        public static void SetStartDate(DependencyObject d, DateTime value) => d.SetValue(StartDateProperty, value);
        public static DateTime GetStartDate(DependencyObject d) => (DateTime)d.GetValue(StartDateProperty);

        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.RegisterAttached(
                "EndDate",
                typeof(DateTime),
                typeof(GanttCanvas),
                new PropertyMetadata(DateTime.Today.AddDays(1), OnGeometryChanged));

        public static void SetEndDate(DependencyObject d, DateTime value) => d.SetValue(EndDateProperty, value);
        public static DateTime GetEndDate(DependencyObject d) => (DateTime)d.GetValue(EndDateProperty);

        public static readonly DependencyProperty RowIndexProperty =
            DependencyProperty.RegisterAttached(
                "RowIndex",
                typeof(int),
                typeof(GanttCanvas),
                new PropertyMetadata(0, OnGeometryChanged));

        public static void SetRowIndex(DependencyObject d, int value) => d.SetValue(RowIndexProperty, value);
        public static int GetRowIndex(DependencyObject d) => (int)d.GetValue(RowIndexProperty);

        public const double RowHeight = 32.0;
        public const double BarPadding = 4.0;

        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement fe)) return;
            UpdateLayout(fe);
        }

        public static void UpdateLayout(FrameworkElement element)
        {
            var start = GetStartDate(element);
            var end = GetEndDate(element);
            var rangeStart = GetRangeStart(element);
            var pxPerDay = GetPixelsPerDay(element);
            var row = GetRowIndex(element);

            var left = (start.Date - rangeStart.Date).TotalDays * pxPerDay;
            var width = Math.Max(1, (end.Date - start.Date).TotalDays) * pxPerDay;

            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, row * RowHeight + BarPadding);
            element.Width = width;
            element.Height = RowHeight - 2 * BarPadding;
        }
    }
}
