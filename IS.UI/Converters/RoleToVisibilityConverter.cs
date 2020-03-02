using IS.Domain.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace IS.UI.Converters
{
    public class RoleToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static RoleToVisibilityConverter _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
            {
                _converter = new RoleToVisibilityConverter();
            }
            return _converter;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Role role = (Role)value;
            int MaxRoleIDAllowed = int.Parse(parameter as string);
            return role.ID <= MaxRoleIDAllowed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)parameter > 400 ? true : false;
    }


}
