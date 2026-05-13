using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkProject.UI.Converters
{
    public class DateToPixelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 3) return 0d;
            if (!(values[0] is DateTime date)) return 0d;
            if (!(values[1] is DateTime rangeStart)) return 0d;
            double pixelsPerDay = 1.0;
            if (values[2] is double d) pixelsPerDay = d;
            else if (values[2] is int i) pixelsPerDay = i;

            var days = (date.Date - rangeStart.Date).TotalDays;
            return days * pixelsPerDay;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
