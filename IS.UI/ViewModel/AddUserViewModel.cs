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
        public ObservableCollection<RoleWrapper> _roles = new ObservableCollection<RoleWrapper>();
        public ObservableCollection<RoleWrapper> Roles { get; set; } = new ObservableCollection<RoleWrapper>();
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
                //OnPropertyChanged(nameof(EditerUser));
                //OnPropertyChanged(nameof(EditerUser.Email));
                //OnPropertyChanged(nameof(EditerUser.Name));
                //OnPropertyChanged(nameof(EditerUser.Role));
            }
        }
        private readonly Context context;
        
        public AddUserViewModel()
        {
            context = new Context();
            context.Roles.ToList().ForEach(x => Roles.Add(new RoleWrapper(x)));
            OnPropertyChanged(nameof(Roles));
            foreach (var item in Roles)
                item.ItemSelected += Item_Selected;
        }

        private void Item_Selected(object _sender, object _SendObject)
        {
            EditerUser = (User)_SendObject;
        }
        public void Add(object obj) 
        {
            m_User.Role = m_SelectedRole;
            SelectedRole.Users.Add(m_User);
            context.Users.Add(m_User);
            context.SaveChanges();
            SelectedRole = new Role();
            EditerUser = new User();
            OnPropertyChanged(nameof(SelectedRole));
            OnPropertyChanged(nameof(EditerUser));
        }
        public ICommand AddUsers { get => new Command.ActionCommand((obj) =>
        {
            if (EditerUser.Validate() && SelectedRole!=null)
            {
                if (EditerUser.ID == 0)
                    context.Add(EditerUser);
                context.SaveChanges();
                Users.Clear();
                context.Users.ToList().ForEach(x => Users.Add(new UsersWrapper(x)));
                foreach (var item in Users)
                    item.ItemSelected += Item_Selected;

                EditerUser = new User();
                OnPropertyChanged(nameof(EditerUser));
                OnPropertyChanged(nameof(EditerUser.Email));
                OnPropertyChanged(nameof(EditerUser.Name));
                OnPropertyChanged(nameof(EditerUser.Role));
                context.SaveChanges();
            }
        }); }
    }
}
