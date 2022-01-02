using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Task10.Converters {
    public class FileSizeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            const long GbSize = 1000000000;
            const int MbSize = 1000000;
            const int KbSize = 1000;

            return (double)value switch {
                >= GbSize => $"{Math.Round((double)value / GbSize, 2)} GB",
                >= MbSize => $"{Math.Round((double)value / MbSize, 2)} MB",
                >= KbSize => $"{Math.Round((double)value / KbSize, 2)} KB",
                _ => $"{value} Byte",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
