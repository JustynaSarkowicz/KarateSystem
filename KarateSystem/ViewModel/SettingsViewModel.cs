using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using KarateSystem.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using KarateSystem.Misc;
using Enum = KarateSystem.Misc.Enum;

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
        public ObservableCollection<string> RoleOptions { get; set; } =
            new ObservableCollection<string>(System.Enum.GetNames(typeof(Enum.UserRole)));

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
            try
            {
                if (!IsUserValid(EditingUser)) return;

                SelectedUser.UserFirstName = EditingUser.UserFirstName;
                SelectedUser.UserLastName = EditingUser.UserLastName;
                SelectedUser.UserLogin = EditingUser.UserLogin;
                SelectedUser.UserPass = EditingUser.UserPass;
                SelectedUser.UserRole = SelectedRole;

                await _userRepository.UpdateUserAsync(SelectedUser);

                Users = new ObservableCollection<UserDto>(await _userRepository.GetAllUsersAsync());
                ExecuteCancelUserCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji użytkownika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExecuteAddUserCommand(object obj)
        {
            try
            {
                if (!IsUserValid(EditingUser)) return;

                EditingUser.UserRole = SelectedRole;
                EditingUser.UserPass = EditingUser.UserPass.Encrypt();

                await _userRepository.AddUserAsync(EditingUser);

                Users = new ObservableCollection<UserDto>(await _userRepository.GetAllUsersAsync());
                ExecuteCancelUserCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania użytkownika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                try 
                { 
                    await _userRepository.DeleteUserAsync(SelectedUser.UserId);
                    
                    Users = new ObservableCollection<UserDto>(await _userRepository.GetAllUsersAsync());
                    ExecuteCancelUserCommand(obj);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas usuwania użytkownika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsUserValid(UserDto user)
        {
            if (user == null ||
                string.IsNullOrWhiteSpace(user.UserFirstName) ||
                string.IsNullOrWhiteSpace(user.UserLastName) ||
                string.IsNullOrWhiteSpace(user.UserLogin) ||
                string.IsNullOrWhiteSpace(user.UserPass) ||
                user.UserPass.Count() < 5 ||
                string.IsNullOrWhiteSpace(SelectedRole))
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.\n Hasło nie może być krótsze niż 5 znaków.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
