using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private User m_CurrentUser;
        public User CurrentUser
        {
            get => m_CurrentUser;
            set
            {

            }
        }

        public async void Login(User _user)
        {
            var user = await context
                .Users
                .Where(x => x.Name == _user.Name)
                .Where(x => x.Password == _user.Password)
                .SingleOrDefaultAsync();
           // if (user is null)
           //     Status = "Combination of user and password was not found";
           // else
           //     Status = $"Loged in as {user.Name}";
        }
    }
}
