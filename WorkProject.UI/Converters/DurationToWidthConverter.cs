using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkProject.UI.Converters
{
    public class DurationToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 3) return 0d;
            if (!(values[0] is DateTime start)) return 0d;
            if (!(values[1] is DateTime end)) return 0d;
            double pixelsPerDay = 1.0;
            if (values[2] is double d) pixelsPerDay = d;
            else if (values[2] is int i) pixelsPerDay = i;

            var days = Math.Max(1, (end.Date - start.Date).TotalDays);
            return days * pixelsPerDay;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
