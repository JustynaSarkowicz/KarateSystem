using KarateSystem.Repository.Interfaces;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using KarateSystem.Views;

namespace KarateSystem.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;

        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }
        #endregion

        public LoginViewModel(IUserRepository userRepository, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;

            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   Username.Length >= 3 &&
                   Password != null &&
                   Password.Length >= 3;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            var user = await _userRepository.AuthenticateUser(
                new NetworkCredential(Username, Password));

            if (user == null)
            {
                ErrorMessage = "Nieprawidłowa nazwa lub hasło.";
                return;
            }

            // Authentication successful
            Thread.CurrentPrincipal = new GenericPrincipal(
                new GenericIdentity(Username), new[] {user.UserRole});

            Application.Current.Dispatcher.Invoke(() =>
            {
                var loginWindow = obj as Window;
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

                Application.Current.MainWindow = mainWindow;
                loginWindow?.Close();

                mainWindow.Show();
            });
        }
    }
}