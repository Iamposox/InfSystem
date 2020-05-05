using IS.Domain;
using IS.Domain.Model;
using IS.UI.Interface;
using IS.UI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IS.UI.ViewModel
{
    public class AddUserViewModel : Abstract.BindableObject
    {
        private readonly Context context;
        private readonly IDataStore<User> dataStore;
        private readonly IDataStore<Role> dataStoreRole;
        private UsersWrapper m_User = new UsersWrapper(new User());
        private Role m_SelectedRole = new Role();
        public ObservableCollection<UsersWrapper> Users { get; set; } = new ObservableCollection<UsersWrapper>();
        public ObservableCollection<Role> ListRoles { get; set; } = new ObservableCollection<Role>();
        public Role SelectedRole
        {
            get => m_SelectedRole;
            set
            {
                m_SelectedRole = value;
            }
        }

        public UsersWrapper EditerUser
        {
            get => m_User;
            set
            {
                m_User = new UsersWrapper((User)value.GetUser.Clone());
                m_SelectedRole = EditerUser.Role;
            }
        }
        public ICommand AddUsersCommand
        {
            get => new Command.ActionCommand(async (obj) => await AddUsers());
        }
        public ICommand CancelCommand { get => new Command.ActionCommand((obj) => ResetEditableSupplier(obj)); }
        public AddUserViewModel()
        {
            context = new Context();
            dataStore = new Service.UsersService(context);
            dataStoreRole = new Service.RoleService(context);
            RefreshRoleListAsync();
            RefreshUserList();
        }
        private async void RefreshRoleListAsync()
        {
            ListRoles.Clear();
            var RolesList = await dataStoreRole.GetItemsAsync();
            RolesList.ToList().ForEach(x => ListRoles.Add(x));
            OnPropertyChanged(nameof(ListRoles));
        }
        private async void RefreshUserList()
        {
            Users.Clear();
            var UsersList = await dataStore.GetItemsAsync();
            UsersList.ToList().ForEach(x =>
            {
                var temp = new UsersWrapper(x);
                temp.ItemSelected += UsersItem_Selected;
                Users.Add(temp);
            });
            OnPropertyChanged(nameof(Users));
        }
        private async void UsersItem_Selected(object _sender, object _SendObject)
        {
            if (_SendObject.ToString() == "Remove")
            {
                var User = _sender as UsersWrapper;
                if (User.GetUser.ID == 1)
                    MessageBox.Show("Нельзя удалить первоначального администратора");
                else
                {
                    if (!await dataStore.DeleteItemAsync(User.GetUser.ID))
                        MessageBox.Show("Ошибка");
                    RefreshUserList();
                    OnPropertyChanged(nameof(Users));
                    OnPropertyChanged(nameof(EditerUser.Name));
                    OnPropertyChanged(nameof(EditerUser.Password));
                    OnPropertyChanged(nameof(EditerUser.Email));
                    OnPropertyChanged(nameof(SelectedRole));
                }
            }
            else
            {
                var temp = _sender as UsersWrapper;
                EditerUser = (UsersWrapper)temp.Clone();
                SelectedRole = EditerUser.Role;
                OnPropertyChanged(nameof(SelectedRole));
                OnPropertyChanged(nameof(EditerUser));
            }
        }
        private void ResetEditableSupplier(object para)
        {
            m_User = new UsersWrapper(new User());
            m_SelectedRole = new Role();
            OnPropertyChanged(nameof(EditerUser));
        }
        private async Task AddUsers()
        {
            if (SelectedRole.RoleName != null)
            {
                if (EditerUser.Role != null)
                {
                    if (EditerUser.GetUser.ID == 1)
                        MessageBox.Show("Нельзя изменить первоначального администратора");
                   //else
                   //{
                   //    var Roles = await dataStoreRole.GetItemAsync(EditerUser.Role.ID);
                   //    Roles.Users.Remove(Roles.Users.FirstOrDefault(x => x.ID == EditerUser.GetUser.ID));
                   //    if (!await dataStoreRole.AddOrUpdateItemAsync(Roles))
                   //        MessageBox.Show("Ошибка");
                   //}
                }
                EditerUser.Role = SelectedRole;
                if (!await dataStore.AddOrUpdateItemAsync(EditerUser.GetUser))
                    MessageBox.Show("Ошибка");
                
                RefreshUserList();
                RefreshRoleListAsync();
                EditerUser = new UsersWrapper(new User());
                OnPropertyChanged(nameof(Users));
                OnPropertyChanged(nameof(EditerUser));
                OnPropertyChanged(nameof(SelectedRole));
            }
            else
            {
                MessageBox.Show($"Вы не выбрали роль для пользователя", "Ошибка");
                EditerUser = new UsersWrapper(new User());
                OnPropertyChanged(nameof(EditerUser));
            }
        }
    }
}
