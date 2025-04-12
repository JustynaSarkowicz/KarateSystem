using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static KarateSystem.Misc.Helper;

namespace KarateSystem.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private bool _isEditingExisting;
        private string _selectedRole;
        private UserDto _selectedUser;
        private UserDto _editingUser;
        private ObservableCollection<UserDto> _users;
        public ObservableCollection<string> RoleOptions { get; set; } = new() { "Admin", "Obsługa" };

        private readonly IUserRepository _userRepository;
        #endregion

        #region Commands
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand CancelUserCommand { get; }
        #endregion

        #region Properties
        public bool IsEditingExisting
        {
            get => _isEditingExisting;
            set
            {
                _isEditingExisting = value;
                OnPropertyChanged(nameof(IsEditingExisting));
                OnPropertyChanged(nameof(IsAddingNew));
            }
        }
        public bool IsAddingNew => !IsEditingExisting;
        public UserDto SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public UserDto EditingUser
        {
            get => _editingUser;
            set
            {
                _editingUser = value;
                OnPropertyChanged(nameof(EditingUser));
            }
        }
        public ObservableCollection<UserDto> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }
        #endregion
        public SettingsViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            EditUserCommand = new ViewModelCommand(ExecuteEditUserCommand, CanEditUser);
            CancelUserCommand = new ViewModelCommand(ExecuteCancelUserCommand);
            UpdateUserCommand = new ViewModelCommand(ExecuteUpdateUserCommand, CanUpdateUser);
            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);
            DeleteUserCommand = new ViewModelCommand(ExecuteDeleteUserCommand, CanEditUser);

            LoadUsers();
        }
        private bool CanEditUser(object obj) => SelectedUser != null;
        private bool CanUpdateUser(object obj) => SelectedUser != null && EditingUser != null;

        private async void LoadUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            Users = new ObservableCollection<UserDto>(users);

            EditingUser = new UserDto();
        }
        private void ExecuteEditUserCommand(object obj)
        {
            if (SelectedUser == null) return;
            EditingUser = new UserDto
            {
                UserId = SelectedUser.UserId,
                UserFirstName = SelectedUser.UserFirstName,
                UserLastName = SelectedUser.UserLastName,
                UserLogin = SelectedUser.UserLogin,
                UserPass = SelectedUser.UserPass.Decrypt(),
                UserRole = SelectedUser.UserRole
            };
            SelectedRole = EditingUser.UserRole;
            IsEditingExisting = true;
        }
        private void ExecuteCancelUserCommand(object obj)
        {
            EditingUser = new UserDto();
            IsEditingExisting = false;
            SelectedUser = null;
            SelectedRole = null;
        }
        private async void ExecuteUpdateUserCommand(object obj)
        {
            if (EditingUser == null || SelectedUser == null) return;
            if (!Users.Contains(SelectedUser)) return;

            SelectedUser.UserFirstName = EditingUser.UserFirstName;
            SelectedUser.UserLastName = EditingUser.UserLastName;
            SelectedUser.UserLogin = EditingUser.UserLogin;
            SelectedUser.UserPass = EditingUser.UserPass;
            SelectedUser.UserRole = EditingUser.UserRole;

            var result = await _userRepository.UpdateUserAsync(SelectedUser);
            if (!result)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var users = await _userRepository.GetAllUsersAsync();
            Users = new ObservableCollection<UserDto>(users);
            ExecuteCancelUserCommand(obj);
        }
        private async void ExecuteAddUserCommand(object obj)
        {
            if(EditingUser == null) return;
            EditingUser.UserRole = SelectedRole;
            EditingUser.UserPass = EditingUser.UserPass.Encrypt();
            var result = await _userRepository.AddUserAsync(EditingUser);
            if (!result)
            {
                MessageBox.Show("Użytkownik o podanym loginie już istnieje", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var users = await _userRepository.GetAllUsersAsync();
            Users = new ObservableCollection<UserDto>(users);
            ExecuteCancelUserCommand(obj);
        }
        private async void ExecuteDeleteUserCommand(object obj)
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show("Czy na pewno chcesz usunąć tego użytkownika?",
                                 "Potwierdzenie usunięcia",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool isDeleted = await _userRepository.DeleteUserAsync(SelectedUser.UserId);
                if (isDeleted)
                {
                    Users.Remove(SelectedUser);
                    var users = await _userRepository.GetAllUsersAsync();
                    Users = new ObservableCollection<UserDto>(users);
                    ExecuteCancelUserCommand(obj);
                }
            }
        }
    }
}
