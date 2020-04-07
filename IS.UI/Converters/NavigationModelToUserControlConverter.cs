using IS.Domain.Model;
using IS.UI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace IS.UI.Converters
{
    public class NavigationModelToUserControlConverter : MarkupExtension, IValueConverter
    {
        private static NavigationModelToUserControlConverter _converter = null;
       
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter is null)
            {
                _converter = new NavigationModelToUserControlConverter();
            }
            return _converter;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NavigationModel navigateTo = (NavigationModel)value;
            if (navigateTo is null) return null;
            if (Manager.ApplicationManager.GetInstance.NavigationNameToUserControl.ContainsKey(navigateTo))
            {
                return Manager.ApplicationManager.GetInstance.NavigationNameToUserControl[navigateTo].Invoke();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }


}
