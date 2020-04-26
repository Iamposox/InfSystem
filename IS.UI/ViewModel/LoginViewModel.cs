using IS.Domain;
using IS.Domain.Model;
using IS.UI.Manager;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IS.UI.ViewModel
{

    /// <summary>
    /// Very inefficiant login View Model
    /// </summary>
    public class LoginViewModel : Abstract.BindableObject
    {
        public User LoginUser { get; set; } = new User();

        public string Password
        {
            get =>
                LoginUser.Password;
            set
            {
                LoginUser.Password = value;
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

        public ICommand Login { get => new Command.ActionCommand((obj) => { ApplicationManager.GetInstance.TryLogin(LoginUser); }); }
        public async Task Logining() { }

    }
}
