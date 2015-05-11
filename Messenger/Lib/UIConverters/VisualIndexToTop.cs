using System;
using System.Globalization;

namespace Messenger.Lib.UIConverters
{
    public class VisualIndexToTop : ConverterBase
    {
        public double ItemHeight { get; set; }
        public double SeparatorHeight { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value * (this.ItemHeight + this.SeparatorHeight);
        }
    }
}