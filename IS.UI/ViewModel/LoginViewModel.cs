using IS.Domain;
using IS.Domain.Model;
using IS.UI.Manager;
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
        public event ClickDelegate Click;
        public User LoginUser 
        { 
            get => m_User;
            set
            {
                m_User = value;
            }
        }

        public string Password
        {
            get =>
                m_User.Password;

            set
            {
                
                m_User.Password = value;
                OnPropertyChanged(nameof(Password));
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

        private ICommand _logicCommand;

        public ICommand Logic =>
            _logicCommand ?? (_logicCommand = new Command.ActionCommand(LogicCommandImplementation));

        private async void LogicCommandImplementation(object parameter)
        {
            var authorizationResult = await ApplicationManager.GetInstance.Login(new System.Tuple<string, string>(LoginUser.Name, LoginUser.Password));
            if (!authorizationResult)
            {
                Status = "Combination of user and password was not found";
            }
            else
            {
                Status = $"Loged in as {LoginUser.Name}";
            }
        }

        private readonly Context context;

        public LoginViewModel()
        {
            context = new Context();
        }

        //This should be moved to Manager class and check if any user is logged in

    }
}
