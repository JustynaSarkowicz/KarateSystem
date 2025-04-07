using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KarateSystem.ViewModel;

public class ClubsDegreesMatsViewModel : ViewModelBase
{
    // Properties
    private Club _selectedClub;
    private Club _editingClub;
    private Mat _selectedMat;
    private Mat _editingMat;
    private Degree _selectedDegree;
    private Degree _editingDegree;
    private string _searchText;
    private ObservableCollection<Club> _clubs;
    private ObservableCollection<Mat> _mats;
    private ObservableCollection<Degree> _degrees;
    private List<Club> _allClubs; 

    // Interfaces
    private readonly IClubRepository _clubRepository;
    private readonly ISearchService _searchService;
    private readonly IMatRepository _matRepository;
    private readonly IDegreeRepository _degreeRepository;

    // Commands
    public ICommand EditClubCommand { get; }
    public ICommand UpdateClubCommand { get; }
    public ICommand AddClubCommand { get; }
    public ICommand CancelClubCommand { get; }
    public ICommand EditMatCommand { get; }
    public ICommand UpdateMatCommand { get; }
    public ICommand AddMatCommand { get; }
    public ICommand CancelMatCommand { get; }
    public ICommand EditDegreeCommand { get; }
    public ICommand UpdateDegreeCommand { get; }
    public ICommand AddDegreeCommand { get; }
    public ICommand CancelDegreeCommand { get; }

    public ObservableCollection<Club> Clubs
    {
        get => _clubs;
        set
        {
            _clubs = value;
            OnPropertyChanged(nameof(Clubs));
        }
    }
    public ObservableCollection<Mat> Mats
    {
        get => _mats;
        set
        {
            _mats = value;
            OnPropertyChanged(nameof(Mats));
        }
    }
    public ObservableCollection<Degree> Degrees
    {
        get => _degrees;
        set
        {
            _degrees = value;
            OnPropertyChanged(nameof(Degrees));
        }
    }
    public Club? SelectedClub
    {
        get => _selectedClub;
        set
        {
            _selectedClub = value;
            OnPropertyChanged(nameof(SelectedClub));
        }
    }
    public Club? EditingClub
    {
        get => _editingClub;
        set
        {
            _editingClub = value;
            OnPropertyChanged(nameof(EditingClub));
        }
    }
    public Mat? EditingMat
    {
        get => _editingMat;
        set
        {
            _editingMat = value;
            OnPropertyChanged(nameof(EditingMat));
        }
    }
    public Mat? SelectedMat
    {
        get => _selectedMat;
        set
        {
            _selectedMat = value;
            OnPropertyChanged(nameof(SelectedMat));
        }
    }
    public Degree? EditingDegree
    {
        get => _editingDegree;
        set
        {
            _editingDegree = value;
            OnPropertyChanged(nameof(EditingDegree));
        }
    }
    public Degree? SelectedDegree
    {
        get => _selectedDegree;
        set
        {
            _selectedDegree = value;
            OnPropertyChanged(nameof(SelectedDegree));
        }
    }
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
            FilterClubs();
        }
    }
    public ClubsDegreesMatsViewModel(IClubRepository clubRepository, 
        ISearchService searchService,
        IMatRepository matRepository,
        IDegreeRepository degreeRepository)
    {
        _clubRepository = clubRepository;
        _searchService = searchService;
        _matRepository = matRepository;
        _degreeRepository = degreeRepository;

        Clubs = new ObservableCollection<Club>();
        EditingClub = new Club();
        Mats = new ObservableCollection<Mat>();
        EditingMat = new Mat();
        Degrees = new ObservableCollection<Degree>();
        EditingDegree = new Degree();

        EditClubCommand = new ViewModelCommand(ExecuteEditClubCommand, CanEditClub);
        UpdateClubCommand = new ViewModelCommand(ExecuteUpdateClubCommand, CanCancelClubEdit);
        AddClubCommand = new ViewModelCommand(ExecuteAddClubCommand);
        CancelClubCommand = new ViewModelCommand(ExecuteCancelClubCommand, CanCancelClubEdit);

        EditMatCommand = new ViewModelCommand(ExecuteEditMatCommand, CanEditMat);
        UpdateMatCommand = new ViewModelCommand(ExecuteUpdateMatCommand, CanCancelMatEdit);
        AddMatCommand = new ViewModelCommand(ExecuteAddMatCommand);
        CancelMatCommand = new ViewModelCommand(ExecuteCancelMatCommand, CanCancelMatEdit);

        EditDegreeCommand = new ViewModelCommand(ExecuteEditDegreeCommand, CanEditDegree);
        UpdateDegreeCommand = new ViewModelCommand(ExecuteUpdateDegreeCommand, CanCancelDegreeEdit);
        AddDegreeCommand = new ViewModelCommand(ExecuteAddDegreeCommand);
        CancelDegreeCommand = new ViewModelCommand(ExecuteCancelDegreeCommand, CanCancelDegreeEdit);


        LoadAsync();
    }

    private async void LoadAsync()
    {
        _allClubs = await _clubRepository.GetAllClubsAsync();
        Clubs = new ObservableCollection<Club>(_allClubs);
        var mats = await _matRepository.GetAllMatAsync();
        Mats = new ObservableCollection<Mat>(mats);
        var degrees = await _degreeRepository.GetAllDegreeAsync();
        Degrees = new ObservableCollection<Degree>(degrees);
    }
    private bool CanEditClub(object obj) => SelectedClub != null;
    private bool CanCancelClubEdit(object obj) => EditingClub != null && SelectedClub != null;
    private bool CanEditMat(object obj) => SelectedMat != null;
    private bool CanCancelMatEdit(object obj) => EditingMat != null && SelectedMat != null;
    private bool CanEditDegree(object obj) => SelectedDegree != null;
    private bool CanCancelDegreeEdit(object obj) => EditingDegree != null && SelectedDegree != null;
    private void ExecuteEditClubCommand(object obj)
    {
        EditingClub = new Club
        {
            ClubId = SelectedClub.ClubId,
            ClubName = SelectedClub.ClubName,
            ClubPlace = SelectedClub.ClubPlace
        };
    }
    private async void ExecuteUpdateClubCommand(object obj)
    {
        try
        {
            if (Clubs.Contains(SelectedClub))
            {
                SelectedClub.ClubName = EditingClub.ClubName;
                SelectedClub.ClubPlace = EditingClub.ClubPlace;
                await _clubRepository.UpdateClubAsync(SelectedClub);
                var index = Clubs.IndexOf(SelectedClub);
                Clubs[index] = SelectedClub;
            }
            var updatedClubs = new ObservableCollection<Club>(Clubs);
            Clubs = updatedClubs;
            EditingClub = new Club();
            SelectedClub = null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating club: {ex.Message}");
        }
    }
    private async void ExecuteAddClubCommand(object obj)
    {
        if (string.IsNullOrWhiteSpace(EditingClub?.ClubName) || string.IsNullOrWhiteSpace(EditingClub?.ClubPlace))
            return;

        var newClub = new Club
        {
            ClubName = EditingClub.ClubName,
            ClubPlace = EditingClub.ClubPlace
        };

        await _clubRepository.AddClubAsync(newClub);
        Clubs.Add(newClub);

        EditingClub = new Club();
    }
    private void ExecuteCancelClubCommand(object obj) => EditingClub = new Club();
    private void ExecuteEditMatCommand(object obj)
    {
        EditingMat = new Mat
        {
            MatId = SelectedMat.MatId,
            MatName = SelectedMat.MatName
        };
    }
    private async void ExecuteUpdateMatCommand(object obj)
    {
        try
        {
            if (Mats.Contains(SelectedMat))
            {
                SelectedMat.MatName= EditingMat.MatName;
                await _matRepository.UpdateMatAsync(SelectedMat);
                var index = Mats.IndexOf(SelectedMat);
                Mats[index] = SelectedMat;
            }
            var updatedMats = new ObservableCollection<Mat>(Mats);
            Mats = updatedMats;
            EditingMat = new Mat();
            SelectedMat = null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating mat: {ex.Message}");
        }
    }
    private async void ExecuteAddMatCommand(object obj)
    {
        if (string.IsNullOrWhiteSpace(EditingMat?.MatName))
            return;

        var newMat = new Mat
        {
            MatName = EditingMat.MatName
        };

        await _matRepository.AddMatAsync(newMat);
        Mats.Add(newMat);

        EditingMat = new Mat();
    }
    private void ExecuteCancelMatCommand(object obj) => EditingMat = new Mat();
    private void ExecuteEditDegreeCommand(object obj)
    {
        EditingDegree = new Degree
        {
            DegreeId = SelectedDegree.DegreeId,
            DegreeName = SelectedDegree.DegreeName
        };
    }
    private async void ExecuteUpdateDegreeCommand(object obj)
    {
        try
        {
            if (Degrees.Contains(SelectedDegree))
            {
                SelectedDegree.DegreeName= EditingDegree.DegreeName;
                await _degreeRepository.UpdateDegreeAsync(SelectedDegree);
                var index = Degrees.IndexOf(SelectedDegree);
                Degrees[index] = SelectedDegree;
            }
            var updatedDegrees = new ObservableCollection<Degree>(Degrees);
            Degrees= updatedDegrees;
            EditingDegree= new Degree();
            SelectedDegree = null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating mat: {ex.Message}");
        }
    }
    private async void ExecuteAddDegreeCommand(object obj)
    {
        if (string.IsNullOrWhiteSpace(EditingDegree?.DegreeName))
            return;

        var newDegree = new Degree
        {
            DegreeName = EditingDegree.DegreeName
        };

        await _degreeRepository.AddDegreeAsync(newDegree);
        Degrees.Add(newDegree);

        EditingDegree = new Degree();
    }
    private void ExecuteCancelDegreeCommand(object obj) => EditingDegree = new Degree();
    private void FilterClubs()
    {
        Clubs = string.IsNullOrWhiteSpace(SearchText)
        ? new ObservableCollection<Club>(_allClubs)
        : new ObservableCollection<Club>(
            _searchService.SearchInCollection(_allClubs, SearchText, "ClubName", "ClubPlace"));
    }
}

