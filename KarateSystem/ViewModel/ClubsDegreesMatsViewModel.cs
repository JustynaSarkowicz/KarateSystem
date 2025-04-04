using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
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
    private ObservableCollection<Club> _clubs;

    // Interfaces
    private readonly IClubRepository _clubRepository;

    // Commands
    public ICommand EditClubCommand { get; }
    public ICommand UpdateClubCommand { get; }
    public ICommand AddClubCommand { get; }
    public ICommand CancelClubCommand { get; }

    public ObservableCollection<Club> Clubs
    {
        get => _clubs;
        set
        {
            _clubs = value;
            OnPropertyChanged(nameof(Clubs));
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

    public ClubsDegreesMatsViewModel(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository ?? throw new ArgumentNullException(nameof(clubRepository));

        Clubs = new ObservableCollection<Club>();
        EditingClub = new Club();

        EditClubCommand = new ViewModelCommand(ExecuteEditClubCommand, CanEditClub);
        UpdateClubCommand = new ViewModelCommand(ExecuteUpdateClubCommand, CanEditClub);
        AddClubCommand = new ViewModelCommand(ExecuteAddClubCommand);
        CancelClubCommand = new ViewModelCommand(ExecuteCancelEditCommand, CanCancelEdit);

        LoadClubsAsync();
    }

    private async void LoadClubsAsync()
    {
        try
        {
            var allClubs = await _clubRepository.GetAllClubsAsync();
            Clubs = new ObservableCollection<Club>(allClubs);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating club: {ex.Message}");
        }
    }
    private void ExecuteEditClubCommand(object obj)
    {
        if (SelectedClub != null)
        {
            EditingClub = new Club
            {
                ClubId = SelectedClub.ClubId,
                ClubName = SelectedClub.ClubName,
                ClubPlace = SelectedClub.ClubPlace
            };
        }
    }

    private bool CanEditClub(object obj) => SelectedClub != null;
    private async void ExecuteUpdateClubCommand(object obj)
    {
        if(SelectedClub == null || EditingClub == null) return;
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
    private void ExecuteCancelEditCommand(object obj) => EditingClub = new Club();
       
    private bool CanCancelEdit(object obj) =>  EditingClub != null && SelectedClub != null;
}

