using IS.Domain.Model;
using IS.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace IS.UI.ViewModel
{
    public class NavigationViewModel
    {
        public ObservableCollection<NavigationModel> NavigationOptions { get; set; } = new ObservableCollection<NavigationModel>
        {
            new NavigationModel("Dashboard",FontAwesome.WPF.FontAwesomeIcon.Globe),
            new NavigationModel("Users",FontAwesome.WPF.FontAwesomeIcon.GoogleWallet),
            new NavigationModel("Customers",FontAwesome.WPF.FontAwesomeIcon.HandScissorsOutline),
            new NavigationModel("Raw Materials",FontAwesome.WPF.FontAwesomeIcon.Heart),
            new NavigationModel("Supplier",FontAwesome.WPF.FontAwesomeIcon.HourglassEnd)
        };
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

        }

    }
}
