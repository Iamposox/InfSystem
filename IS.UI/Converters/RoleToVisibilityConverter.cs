using IS.Domain.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace IS.UI.Converters
{
    public class RoleToVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
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
            Role role = Manager.ApplicationManager.GetInstance.CurrentUser?.Role;
            if(parameter.ToString().ToLower() == "any")
                return Visibility.Visible;
            if(role is null)
                return Visibility.Collapsed;
            if (parameter.ToString().Contains('|'))
            {
                foreach (var item in parameter.ToString().Split('|').ToList())
                {
                    if(int.TryParse(item, out int ID))
                    {
                        if (role.ID == ID) return Visibility.Visible;
                    }
                }
            }
            if (int.TryParse(parameter as string, out int MaxRoleIDAllowed))
            {
                return role.ID <= MaxRoleIDAllowed?Visibility.Visible: Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Role role = Manager.ApplicationManager.GetInstance.CurrentUser?.Role;
            if (values[0].ToString().ToLower() == "any")
                return Visibility.Visible;
            if (role is null)
                return Visibility.Collapsed;
            if (values[0].ToString().Contains('|'))
            {
                foreach (var item in values[0].ToString().Split('|').ToList())
                {
                    if (int.TryParse(item, out int ID))
                    {
                        if (role.ID == ID) return Visibility.Visible;
                    }
                }
            }
            if (int.TryParse(values[0] as string, out int MaxRoleIDAllowed))
            {
                return role.ID <= MaxRoleIDAllowed ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }


}
