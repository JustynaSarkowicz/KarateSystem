using FontAwesome.Sharp;
using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;

namespace KarateSystem.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private ViewModelBase _currentChildView;
        private UserDto _user;
        private string _caption;
        private IconChar _icon;
        public bool IsAdmin => User?.UserRole == "Admin";
        public bool IsService => User?.UserRole == "Obsługa" || User?.UserRole == "Admin";


        private readonly CompetitorsViewModel _competitorsViewModel;
        private readonly ClubsDegreesMatsViewModel _clubsDegreesMatsViewModel;
        private readonly CategoryViewModel _categoryViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly TournamentViewModel _tournamentViewModel;
        private readonly HomeViewModel _homeViewModel;
        private readonly KataKumiteViewModel _kataKumiteViewModel;
        private readonly ResultAnalysisViewModel _resultAnalysisViewModel;

        private readonly IUserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        public UserDto User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(IsService));
            }
        }
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }

            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }

            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }

            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        #endregion

        #region Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCompetitorViewCommand { get; }
        public ICommand ShowClubDegreesMatsViewCommand { get; }
        public ICommand ShowCategoryViewCommand { get; }
        public ICommand ShowTournamentViewCommand { get; }
        public ICommand ShowKataKumiteViewCommand { get; }
        public ICommand ShowSettingsViewCommand { get; }
        public ICommand ShowResultAnalysisViewCommand { get; }
        public ICommand LogoutCommand { get; }
        #endregion
        public MainViewModel(CompetitorsViewModel competitorsViewModel,
            ClubsDegreesMatsViewModel clubsDegreesMatsViewModel,
            CategoryViewModel categoryViewModel,
            SettingsViewModel settingsViewModel,
            TournamentViewModel tournamentViewModel,
            HomeViewModel homeViewModel,
            KataKumiteViewModel kataKumiteViewModel,
            ResultAnalysisViewModel resultAnalysisViewModel,
            IUserRepository userRepository,
            IServiceProvider serviceProvider)
        {
            _competitorsViewModel = competitorsViewModel;
            _clubsDegreesMatsViewModel = clubsDegreesMatsViewModel;
            _categoryViewModel = categoryViewModel;
            _settingsViewModel = settingsViewModel;
            _userRepository = userRepository;
            _tournamentViewModel = tournamentViewModel;
            _homeViewModel = homeViewModel;
            _kataKumiteViewModel = kataKumiteViewModel;
            _serviceProvider = serviceProvider;
            _resultAnalysisViewModel = resultAnalysisViewModel;

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCompetitorViewCommand = new ViewModelCommand(ExecuteShowCompetitorViewCommand);
            ShowClubDegreesMatsViewCommand = new ViewModelCommand(ExecuteShowClubDegreesMatsViewCommand);
            ShowCategoryViewCommand = new ViewModelCommand(ExecuteShowCategoryViewCommand);
            ShowTournamentViewCommand = new ViewModelCommand(ExecuteShowTournamentViewCommand);
            ShowKataKumiteViewCommand = new ViewModelCommand(ExecuteShowKataKumiteViewCommand);
            ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);
            ShowResultAnalysisViewCommand = new ViewModelCommand(ExecuteShowResultAnalysisViewCommand);

            LoadCurrentUserData();
            ExecuteShowHomeViewCommand(null);
        }

        private async void LoadCurrentUserData()
        {
            var identity = Thread.CurrentPrincipal?.Identity;
            if (identity == null || !identity.IsAuthenticated)
            {
                App.Current.Shutdown();
                return;
            }
            var user = await _userRepository.GetUserDtoByName(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                User = new UserDto()
                {
                    UserFirstName = user.UserFirstName,
                    UserLastName = user.UserLastName,
                    UserRole = user.UserRole
                };
            }
            else
            {
                MessageBox.Show("Nie prawidłowoy użytkownik, nie zalogowano.");
                App.Current.Shutdown();
            }
        }
        private void ExecuteLogoutCommand(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var currentWindow = obj as Window;
                var loginWindow = _serviceProvider.GetRequiredService<LoginView>();

                Application.Current.MainWindow = loginWindow;
                currentWindow?.Close();

                loginWindow.Show();
            });
        }
        private void ExecuteShowCompetitorViewCommand(object obj)
        {
            CurrentChildView = _competitorsViewModel;
            Caption = "Zawodnicy";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = _homeViewModel;
            Caption = "Strona Główna";
            Icon = IconChar.Home;
        }

        private void ExecuteShowClubDegreesMatsViewCommand(object obj)
        {
            CurrentChildView = _clubsDegreesMatsViewModel;
            Caption = "Kluby, Stopnie, Maty";
            Icon = IconChar.Pen;
        }
        private void ExecuteShowCategoryViewCommand(object obj)
        {
            CurrentChildView = _categoryViewModel;
            Caption = "Kategorie";
            Icon = IconChar.Navicon;
        }
        private void ExecuteShowTournamentViewCommand(object obj)
        {
            CurrentChildView = _tournamentViewModel;
            Caption = "Turnieje";
            Icon = IconChar.Trophy;
        }
        private void ExecuteShowKataKumiteViewCommand(object obj)
        {
            CurrentChildView = _kataKumiteViewModel;
            Caption = "Kata i Kumite";
            Icon = IconChar.Medal;
        }
        private void ExecuteShowSettingsViewCommand(object obj)
        {
            CurrentChildView = _settingsViewModel;
            Caption = "Ustawienia";
            Icon = IconChar.Cog;
        }
        private void ExecuteShowResultAnalysisViewCommand(object obj)
        {
            CurrentChildView = _resultAnalysisViewModel;
            Caption = "Wyniki i analiza";
            Icon = IconChar.Cog;
        }
    }
}
