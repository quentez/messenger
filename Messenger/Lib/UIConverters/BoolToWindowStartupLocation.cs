using System;
using System.Globalization;
using System.Windows;

namespace Messenger.Lib.UIConverters
{
    public class BoolToWindowStartupLocation : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
        }
    }
}