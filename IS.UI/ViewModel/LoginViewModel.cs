using IS.Domain;
using IS.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;

namespace IS.UI.ViewModel
{

    /// <summary>
    /// Very inefficiant login View Model
    /// </summary>
    public class LoginViewModel : Abstract.BindableObject
    {
        private User m_User = new User();
        public User LoginUser 
        { 
            get => m_User;
            set
            {
                m_User = value;
            }
        }

        private string m_Status = string.Empty;
        public string Status 
        {
            get => m_Status;
            set
            {
                m_Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ICommand Logic { get => new Command.ActionCommand((obj) =>Login(obj)); }

        private readonly Context context;

        public LoginViewModel()
        {
            context = new Context();
        }

        //This should be moved to Manager class and check if any user is logged in
        private async void Login(object obj)
        {
            var user = await context
                .Users
                .Where(x=>x.Name == LoginUser.Name)
                .Where(x=>x.Password == LoginUser.Password)
                .SingleOrDefaultAsync();
            if (user is null)
                Status = "Combination of user and password was not found";
            else
                Status = $"Loged in as {user.Name}";
        }
    }
}
