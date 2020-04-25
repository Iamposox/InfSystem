using IS.Domain.Model;
using IS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Model
{
    public class UsersWrapper: ICloneable
    {
        private User m_User;
        public event SelectedItemDelegate ItemSelected;
        public UsersWrapper(User _user)
        {
            m_User = _user;
        }
        public User GetUser { get => m_User; }
        public string Name { get => m_User.Name; set => m_User.Name = value; }
        public string Email { get => m_User.Email; set => m_User.Email = value; }
        public string Password { get => m_User.Password; set => m_User.Password = value; }
        public ICommand Selected
        {
            get => new Command.ActionCommand((obj) =>
            {
                ItemSelected?.Invoke(this, obj);
            });
        }
        public Role Role
        {
            get => m_User.Role;
            set
            {
                m_User.Role = value;
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
