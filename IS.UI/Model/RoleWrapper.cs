using IS.Domain.Model;
using IS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class RoleWrapper
    {
        private Role m_Role;
        public event SelectedItemDelegate ItemSelected;
        public RoleWrapper(Role _role)
        {
            m_Role= _role;
        }
        public Role Role { get => m_Role; set => m_Role = value; }
        public string Name
        {
            get => m_Role.RoleName;
            set
            {
                m_Role.RoleName = value;
            }
        }

        public User[] Users =>
            m_Role.Users.ToArray();

        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, m_Role);
            });
        }
    }
}
