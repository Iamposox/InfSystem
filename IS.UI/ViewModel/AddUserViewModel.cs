using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class AddUserViewModel: Abstract.BindableObject
    {
        private User m_User;
        private Role m_SelectedRole;
        public Role SelectedRole 
        {
            get => m_SelectedRole;
            set
            {
                m_SelectedRole = value;
            }
        }
        public List<Role> roles { get=>context.Roles.ToList();}
        public ICommand AddUsers { get => new Command.ActionCommand((obj) => Add(obj)); }
        public User AddUser
        { 
            get=>m_User; 
            set 
            {
                m_User = value;
            }
        }
        private readonly Context context;
        public AddUserViewModel()
        {
            context = new Context();
        }

        public void Add(object obj) 
        {
            m_User.Role = m_SelectedRole;
            SelectedRole.Users.Add(m_User);
            context.Users.Add(m_User);
            context.SaveChanges();
            SelectedRole = new Role();
            AddUser = new User();
            OnPropertyChanged(nameof(SelectedRole));
            OnPropertyChanged(nameof(AddUser));
        }
    }
}
