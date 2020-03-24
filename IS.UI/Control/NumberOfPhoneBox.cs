using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace IS.UI.Control
{
    public class NumberOfPhoneBox:TextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            var text = Text;
            //\d\-\(\d{3}\)\-\d{3}(\d{2}){2}
            Regex reg = new Regex("/d/-/(/d{3}/)/-/d{3}(/d{2}){2}");
            base.OnTextChanged(e);
            if (!double.TryParse(Text, out double a) && !String.IsNullOrEmpty(Text))
            {
                Text = text;
                Background = System.Windows.Media.Brushes.Red;
            }
            else
            {
                Background = System.Windows.Media.Brushes.White;
            }
        }
    }
}
