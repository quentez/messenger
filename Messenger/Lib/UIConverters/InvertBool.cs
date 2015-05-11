using System;
using System.Globalization;

namespace Messenger.Lib.UIConverters
{
    public class InvertBool : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}