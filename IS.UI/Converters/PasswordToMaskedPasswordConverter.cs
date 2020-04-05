using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace IS.UI.Converters
{
    public sealed class PasswordToMaskedPasswordConverter : MarkupExtension, IValueConverter
    {
        private static PasswordToMaskedPasswordConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
            {
                _converter = new PasswordToMaskedPasswordConverter();
            }
            return _converter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || !(value is string))
            {
                return null;
            }
            return string.Concat((value as string).Select(c => '*'));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
