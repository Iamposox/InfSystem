using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Manager
{

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


    }
}
