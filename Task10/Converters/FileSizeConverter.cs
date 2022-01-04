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
            const int NumbersAfterComma = 2;

            double val = (double)value;

            return val switch {
                >= GbSize => $"{Math.Round(val / GbSize, NumbersAfterComma)} GB",
                >= MbSize => $"{Math.Round(val / MbSize, NumbersAfterComma)} MB",
                >= KbSize => $"{Math.Round(val / KbSize, NumbersAfterComma)} KB",
                _ => $"{value} Byte",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
