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
        private User m_Add = new User();
        public User AddUser
        { 
            get=>m_Add; 
            set 
            {
                m_Add = value;
            }
        }
        private readonly Context context;
        public AddUserViewModel()
        {
            context = new Context();
        }
        public List<Role> roles { get=>context.Roles.ToList();}
        public ICommand AddUsers { get => new Command.ActionCommand((obj) => Add(obj)); }
        public void Add(object obj) 
        {
            var user = context.Users.Add(AddUser);
        }
    }
}
