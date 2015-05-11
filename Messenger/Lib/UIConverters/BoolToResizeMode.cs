using System;
using System.Globalization;
using System.Windows;

namespace Messenger.Lib.UIConverters
{
    public class BoolToResizeMode : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ResizeMode.CanResize : ResizeMode.CanMinimize;
        }
    }
}