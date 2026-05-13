using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WorkProject.UI.Controls.Gantt
{
    public class TimelineHeader : Canvas
    {
        public static readonly DependencyProperty RangeStartProperty =
            DependencyProperty.Register(nameof(RangeStart), typeof(DateTime), typeof(TimelineHeader),
                new FrameworkPropertyMetadata(DateTime.Today,
                    FrameworkPropertyMetadataOptions.AffectsRender, OnAnyChanged));

        public static readonly DependencyProperty RangeEndProperty =
            DependencyProperty.Register(nameof(RangeEnd), typeof(DateTime), typeof(TimelineHeader),
                new FrameworkPropertyMetadata(DateTime.Today.AddMonths(3),
                    FrameworkPropertyMetadataOptions.AffectsRender, OnAnyChanged));

        public static readonly DependencyProperty PixelsPerDayProperty =
            DependencyProperty.Register(nameof(PixelsPerDay), typeof(double), typeof(TimelineHeader),
                new FrameworkPropertyMetadata(20.0,
                    FrameworkPropertyMetadataOptions.AffectsRender, OnAnyChanged));

        public DateTime RangeStart
        {
            get => (DateTime)GetValue(RangeStartProperty);
            set => SetValue(RangeStartProperty, value);
        }

        public DateTime RangeEnd
        {
            get => (DateTime)GetValue(RangeEndProperty);
            set => SetValue(RangeEndProperty, value);
        }

        public double PixelsPerDay
        {
            get => (double)GetValue(PixelsPerDayProperty);
            set => SetValue(PixelsPerDayProperty, value);
        }

        public TimelineHeader()
        {
            Height = 40;
            Background = Brushes.WhiteSmoke;
            Rebuild();
        }

        private static void OnAnyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineHeader h) h.Rebuild();
        }

        private void Rebuild()
        {
            Children.Clear();
            if (RangeEnd <= RangeStart) return;
            var totalDays = (RangeEnd.Date - RangeStart.Date).TotalDays;
            Width = totalDays * PixelsPerDay;

            for (int i = 0; i <= totalDays; i++)
            {
                var x = i * PixelsPerDay;
                var day = RangeStart.Date.AddDays(i);

                var line = new Line
                {
                    X1 = x, X2 = x, Y1 = 20, Y2 = 40,
                    Stroke = Brushes.LightGray, StrokeThickness = 0.5
                };
                Children.Add(line);

                if (day.DayOfWeek == DayOfWeek.Monday || i == 0)
                {
                    var weekLabel = new TextBlock
                    {
                        Text = day.ToString("MM/dd"),
                        FontSize = 10,
                        Foreground = Brushes.Black
                    };
                    SetLeft(weekLabel, x + 2);
                    SetTop(weekLabel, 22);
                    Children.Add(weekLabel);
                }

                if (day.Day == 1 || i == 0)
                {
                    var monthLabel = new TextBlock
                    {
                        Text = day.ToString("yyyy-MM"),
                        FontSize = 11,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.DarkSlateGray
                    };
                    SetLeft(monthLabel, x + 2);
                    SetTop(monthLabel, 2);
                    Children.Add(monthLabel);
                }
            }
        }
    }
}
