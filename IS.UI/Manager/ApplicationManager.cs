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
    public delegate void ClickDelegate(object _sender, object _sendObject);
    /// <summary>
    /// Simple Thread Safe Singleton
    /// Application Manager Stores global accessable elements in application
    /// </summary>
    public class ApplicationManager
    {
        private static ApplicationManager instance;
        private static readonly object padlock = new object();
        private readonly Context context;
        public LoginViewModel GetUser { get; private set; } = new LoginViewModel();
        private ApplicationManager()
        {
            context = new Context();
            SetTestingUser();
        }
        private void SetTestingUser()
        {
            CurrentUser = context.Users.Include(x => x.Role).Where(x => x.Role.RoleName == "Guest").SingleOrDefault();
            //Login(CurrentUser);
        }
        public string Status { get; set; }
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
        internal async Task<bool> Login(object obj)
        {
            var name = default(string);
            var password = default(string);
            if (!(obj is null))
            {
                var param = obj as Tuple<string, string>;
                (name, password) = (param.Item1, param.Item2);
            }
            else
            {
                (name, password) = (CurrentUser.Name, CurrentUser.Password);
            }
            var user = await context
                .Users
                .Where(x => x.Name == name && x.Password == password)
                .SingleOrDefaultAsync();
            CurrentUser = user;
            if (user is null)
            {
                
                return false;
            }
            else
            {
                Status = $"Loged in as {user.Name}";
                return true;
            }
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        public event NavigationDelegate NewNavigationRequested;

        public void RaiseNavigationEven(object _sender, NavigationModel _navigateTo)
            => NewNavigationRequested?.Invoke(_sender, _navigateTo);

        /// <summary>
        /// Mapping Navigation names to actual Views
        /// </summary>
        public static Dictionary<NavigationModel, UserControl> NavigationNameToUserControl = new Dictionary<NavigationModel, UserControl>
        {
            {new NavigationModel("Dashboard",FontAwesome.WPF.FontAwesomeIcon.Globe), new View.AddUserView()},
            {new NavigationModel("Users",FontAwesome.WPF.FontAwesomeIcon.GoogleWallet), new View.AddUserView()},
            {new NavigationModel("Customers",FontAwesome.WPF.FontAwesomeIcon.HandScissorsOutline), new View.CustomersViewTwo()},
            {new NavigationModel("Raw Materials",FontAwesome.WPF.FontAwesomeIcon.Heart),new View.RawMaterialsVIew()},
            {new NavigationModel("Supplier",FontAwesome.WPF.FontAwesomeIcon.HourglassEnd),new View.SupplierView()},
            {new NavigationModel("Asortiments",FontAwesome.WPF.FontAwesomeIcon.Fire),new View.AssortmentsView()},
        };

    }
}
