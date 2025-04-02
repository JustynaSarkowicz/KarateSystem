using FontAwesome.Sharp;
using KarateSystem.Repository.Interfaces;
using System.Windows.Input;

namespace KarateSystem.ViewModel;

public class MainViewModel : ViewModelBase
{
    //Fields
    private ViewModelBase _currentChildView;
    private string _caption;
    private IconChar _icon;

    private readonly CompetitorsViewModel _competitorsViewModel;
    private readonly ClubsDegreesMatsViewModel _clubsDegreesMatsViewModel;
    private readonly ICompetitorRepository _competitorRepository;

    //Properties
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

    //--> Commands
    public ICommand ShowHomeViewCommand { get; }
    public ICommand ShowCompetitorViewCommand { get; }
    public ICommand ShowClubDegreesMatsViewCommand { get; }
    public ICommand ShowCategoryViewCommand { get; }
    public ICommand ShowTournamentViewCommand { get; }
    public ICommand ShowKataKumiteViewCommand { get; }
    public ICommand ShowSettingsViewCommand { get; }

    public MainViewModel(CompetitorsViewModel competitorsViewModel,
        ClubsDegreesMatsViewModel clubsDegreesMatsViewModel)
    {
        _competitorsViewModel = competitorsViewModel;
        _clubsDegreesMatsViewModel = clubsDegreesMatsViewModel;

        ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
        ShowCompetitorViewCommand = new ViewModelCommand(ExecuteShowCompetitorViewCommand);
        ShowClubDegreesMatsViewCommand = new ViewModelCommand(ExecuteShowClubDegreesMatsViewCommand);
        ShowCategoryViewCommand = new ViewModelCommand(ExecuteShowCategoryViewCommand);
        ShowTournamentViewCommand = new ViewModelCommand(ExecuteShowTournamentViewCommand);
        ShowKataKumiteViewCommand = new ViewModelCommand(ExecuteShowKataKumiteViewCommand);
        ShowSettingsViewCommand = new ViewModelCommand(ExecuteShowSettingsViewCommand);

        ExecuteShowHomeViewCommand(null);
    }

    private void ExecuteShowCompetitorViewCommand(object obj)
    {
        CurrentChildView = _competitorsViewModel;
        Caption = "Zawodnicy";
        Icon = IconChar.UserGroup;
    }

    private void ExecuteShowHomeViewCommand(object obj)
    {
        CurrentChildView = new HomeViewModel();
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
        CurrentChildView = new CategoryViewModel();
        Caption = "Kategorie";
        Icon = IconChar.Navicon;
    }
    private void ExecuteShowTournamentViewCommand(object obj)
    {
        CurrentChildView = new TournamentViewModel();
        Caption = "Turnieje";
        Icon = IconChar.Trophy;
    }
    private void ExecuteShowKataKumiteViewCommand(object obj)
    {
        CurrentChildView = new KataKumiteViewModel();
        Caption = "Kata i Kumite";
        Icon = IconChar.Medal;
    }
    private void ExecuteShowSettingsViewCommand(object obj)
    {
        CurrentChildView = new SettingsViewModel();
        Caption = "Ustawienia";
        Icon = IconChar.Cog;
    }
}
