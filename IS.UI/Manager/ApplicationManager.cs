using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using IS.UI.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace IS.UI.Manager
{
    public delegate void NavigationDelegate(object _sender, NavigationModel _navigateTo);
    public delegate void UpdateValuedNotificationDelegate(object _sender);
    /// <summary>
    /// Simple Thread Safe Singleton
    /// Application Manager Stores global accessable elements in application
    /// </summary>
    public class ApplicationManager
    {
        private static ApplicationManager instance;
        private static readonly object padlock = new object();
        private readonly Context context;
        
        private ApplicationManager()
        {
            context = new Context();
        }

        public static ApplicationManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ApplicationManager();
                        }
                    }
                }
                return instance;
            }
        }

        public User CurrentUser { get; private set; }

        public bool TryLogin(User _loginData)
        {
            var user = context.Users
                .Where(x => x.Name == _loginData.Name)
                .Where(x => x.Password == _loginData.Password)
                .Include(x=>x.Role)
                .FirstOrDefault();
            if (user is null)
                return false;
            CurrentUser = user;
            ValuesChangedNotification?.Invoke(this);
            return true;
        }

        public void LogOut()
        {
            CurrentUser = null;
            ValuesChangedNotification?.Invoke(this);
        }

        public event NavigationDelegate NewNavigationRequested;

        public void RaiseNavigationEven(object _sender, NavigationModel _navigateTo) 
            => NewNavigationRequested?.Invoke(_sender, _navigateTo);

        public event UpdateValuedNotificationDelegate ValuesChangedNotification;

        /// <summary>
        /// Mapping Navigation names to actual Views
        /// </summary>
        public Dictionary<NavigationModel, Func<UserControl>> NavigationNameToUserControl =
            new Dictionary<NavigationModel, Func<UserControl>>
        {
            {new NavigationModel("Главное",FontAwesome.WPF.FontAwesomeIcon.Globe,"any"),()=> new View.DashboardView()},
            {new NavigationModel("Пользователи",FontAwesome.WPF.FontAwesomeIcon.GoogleWallet,"1|2|3|4|5"), ()=>new View.UsersView()},
            {new NavigationModel("Клиенты",FontAwesome.WPF.FontAwesomeIcon.HandScissorsOutline,"1|2|3|4|5"),()=> new View.CustomersViewTwo()},
            {new NavigationModel("Продукты",FontAwesome.WPF.FontAwesomeIcon.Heart,"1|2|3|4|5"),()=>new View.RawMaterialsVIew()},
            {new NavigationModel("Поставщики",FontAwesome.WPF.FontAwesomeIcon.HourglassEnd,"1|2|3|4|5"),()=>new View.SupplierView()},
            {new NavigationModel("Ассортимент",FontAwesome.WPF.FontAwesomeIcon.Fire,"1|2|3|4|5"),()=>new View.AssortmentsView()},
        };

    }
}
