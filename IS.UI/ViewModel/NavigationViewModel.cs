using IS.Domain.Model;
using IS.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace IS.UI.ViewModel
{
    public class NavigationViewModel
    {
        public ObservableCollection<NavigationModel> NavigationOptions { get; set; }
        private NavigationModel m_SelectedModel;

        public NavigationModel SelectedItem
        {
            get { return m_SelectedModel; }
            set
            {
                m_SelectedModel = value;
                Manager.ApplicationManager.GetInstance.RaiseNavigationEven(this, value);
            }
        }

        public NavigationViewModel()
        {
            NavigationOptions = new ObservableCollection<NavigationModel>(Manager.ApplicationManager.NavigationNameToUserControl.Keys.ToList());
        }

    }
}
