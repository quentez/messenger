using System;
using System.Globalization;
using System.Windows.Data;

namespace Messenger.Lib.UIConverters
{
    public abstract class ConverterBase : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
