using System;
using System.Globalization;
using System.Windows;

namespace Messenger.Lib.UIConverters
{
    public class BoolToWindowState : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? WindowState.Maximized : WindowState.Normal;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (WindowState)value == WindowState.Maximized;
        }
    }
}