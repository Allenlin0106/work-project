using System;
using System.Globalization;
using System.Windows.Data;

namespace WorkProject.UI.Converters
{
    public class ProgressToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return 0d;
            int progress = 0;
            if (values[0] is int i) progress = i;
            double totalWidth = 0;
            if (values[1] is double d) totalWidth = d;
            progress = Math.Max(0, Math.Min(100, progress));
            return totalWidth * progress / 100.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
