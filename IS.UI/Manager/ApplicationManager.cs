using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IS.UI.Manager
{
    public delegate void NavigationDelegate(object _sender, NavigationModel _navigateTo);
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

        public async Task<User> Login(User _user)
        {
            if (CurrentUser is null)
            {
                var temp = await context
                    .Users
                    .Where(x => x.Name == _user.Name)
                    .Where(x => x.Password == _user.Password)
                    .SingleOrDefaultAsync();

                if (temp != null)
                    CurrentUser = temp;
            }
            return CurrentUser;
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        public event NavigationDelegate NewNavigationRequested;

        public void RaiseNavigationEven(object _sender, NavigationModel _navigateTo)
            => NewNavigationRequested?.Invoke(_sender, _navigateTo);

        public static Dictionary<NavigationModel, UserControl> NavigationNameToUserControl = new Dictionary<NavigationModel, UserControl>
        {
            {new NavigationModel("Dashboard",FontAwesome.WPF.FontAwesomeIcon.Globe), new View.AddUserView()},
            {new NavigationModel("Users",FontAwesome.WPF.FontAwesomeIcon.GoogleWallet), new View.AddUserView()},
            {new NavigationModel("Customers",FontAwesome.WPF.FontAwesomeIcon.HandScissorsOutline), new View.CustomerView()},
            {new NavigationModel("Raw Materials",FontAwesome.WPF.FontAwesomeIcon.Heart),new View.RawMaterialsVIew()},
            {new NavigationModel("Supplier",FontAwesome.WPF.FontAwesomeIcon.HourglassEnd),new View.SupplierView()}
        };

    }
}
