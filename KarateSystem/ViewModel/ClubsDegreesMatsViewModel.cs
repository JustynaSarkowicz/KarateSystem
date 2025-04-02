using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KarateSystem.ViewModel
{
    public class ClubsDegreesMatsViewModel : ViewModelBase
    {
        // Properties
        private Club selectedClub;
        private ObservableCollection<Club> clubs;

        // Commands
        private ICommand _editClubCommand;
        public ICommand EditClubCommand => _editClubCommand ??= new ViewModelCommand(ExecuteEditClubCommand);

        // Interfaces
        private readonly IClubRepository _clubRepository;
        public ObservableCollection<Club> Clubs
        {
            get => clubs;
            set
            {
                clubs = value;
                OnPropertyChanged(nameof(Clubs));
            }
        }
        public Club SelectedClub
        {
            get => selectedClub;
            set
            {
                selectedClub = value;
                OnPropertyChanged(nameof(SelectedClub));
            }
        }
        public ClubsDegreesMatsViewModel(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
            LoadClubsAsync();
        }
        // CLubs
        private async void LoadClubsAsync()
        {
            var allClubs = await _clubRepository.GetAllClubsAsync();
            Clubs = new ObservableCollection<Club>(allClubs);
        }
        private async void ExecuteEditClubCommand(object obj)
        {
            if (SelectedClub == null) return;

            try
            {
                await _clubRepository.UpdateClubAsync(SelectedClub);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating club: {ex.Message}");
            }
        }
        //Mats

        // Degrees
    }
}
