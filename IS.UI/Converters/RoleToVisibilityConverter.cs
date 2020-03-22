using IS.Domain.Model;
using System;
using System.Globalization;
using System.Windows;
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
            Role role = Manager.ApplicationManager.GetInstance.CurrentUser.Role;//(Role)value;
            if (int.TryParse(parameter as string, out int MaxRoleIDAllowed))
            {
                return role.ID <= MaxRoleIDAllowed?Visibility.Visible: Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }


}
