using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IS.UI.Abstract
{
    //Hello World
    /// <summary>
    /// This class is used as based class for objects which need to informa UI over PropertyChanged
    /// </summary>
    public abstract class BindableObject : INotifyPropertyChanged
    {
        protected void UpdatePropperties(List<string> _propertyNameList)
        {
            foreach (var item in _propertyNameList)
            {
                OnPropertyChanged(item);
            }
        }

        protected void OnPropertyChanged(string _name)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(_name));
        }

        protected void SetProperty<T>(ref T _item, T _value, [CallerMemberName] string _name = null)
        {
            if (EqualityComparer<T>.Default.Equals(_item, _value))
            {
                _item = _value;
                OnPropertyChanged(_name);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
