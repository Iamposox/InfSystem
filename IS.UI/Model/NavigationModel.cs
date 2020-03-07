﻿using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Text;

namespace IS.UI.Model
{
    public class NavigationModel
    {
        public NavigationModel(string _title,FontAwesomeIcon _icon)
        {
            Title = _title;
            Icon = _icon;
        }
        public string Title { get; set; }
        public FontAwesomeIcon Icon { get; set; }

        public override bool Equals(object obj)
        {
            return obj is NavigationModel model &&
                   Title == model.Title &&
                   Icon == model.Icon;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Icon);
        }
    }
}
