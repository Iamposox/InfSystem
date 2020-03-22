using System;
using System.Windows.Controls;

namespace IS.UI.Control
{
    public class NumericBox : TextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            var temp = Text;
            base.OnTextChanged(e);
            if(!double.TryParse(Text, out double a) && !String.IsNullOrEmpty(Text))
            {
                Text = temp;
                Background = System.Windows.Media.Brushes.Red;
            }
            else
            {
                Background = System.Windows.Media.Brushes.White;
            }
        }
    }
}
