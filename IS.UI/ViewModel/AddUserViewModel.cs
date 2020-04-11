using IS.Domain;
using IS.Domain.Model;
using IS.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class AddUserViewModel: Abstract.BindableObject
    {
        private User m_User = new User();
        private Role m_SelectedRole;
        public ObservableCollection<UsersWrapper> Users { get; set; } = new ObservableCollection<UsersWrapper>();
        public List<Role> ListRoles { get => context.Roles.ToList(); } 
        public Role SelectedRole 
        {
            get => m_SelectedRole;
            set
            {
                m_SelectedRole = value;
            }
        }
        
        public User EditerUser
        { 
            get=>m_User; 
            set 
            {
                m_User = value;
                m_SelectedRole = value.Role;
                Change();
            }
        }
        private readonly Context context;
        
        public AddUserViewModel()
        {
            context = new Context();
            context.Users.ToList().ForEach(x => Users.Add(new UsersWrapper(x)));
            foreach (var item in Users)
                item.ItemSelected += Item_Selected;
        }

        private void Item_Selected(object _sender, object _SendObject)
        {
            EditerUser = (User)_SendObject;
        }
        public void Change()
        {
            OnPropertyChanged(nameof(EditerUser));
            OnPropertyChanged(nameof(EditerUser.Email));
            OnPropertyChanged(nameof(EditerUser.Name));
            OnPropertyChanged(nameof(EditerUser.Role));
            OnPropertyChanged(nameof(EditerUser.Password));
            OnPropertyChanged(nameof(SelectedRole));
            OnPropertyChanged(nameof(SelectedRole.RoleName));
        }
        public ICommand AddUsers { get => new Command.ActionCommand((obj) =>
        {
            if (EditerUser.Validate() && SelectedRole!=null)
            {
                if (EditerUser.ID == 0)
                    context.Add(EditerUser);
                context.SaveChanges();
                Users.Clear();
                EditerUser.Role = SelectedRole;
                context.Users.ToList().ForEach(x => Users.Add(new UsersWrapper(x)));
                foreach (var item in Users)
                    item.ItemSelected += Item_Selected;

                EditerUser = new User();
                Change();
                context.SaveChanges();
            }
        }); }
    }
}
