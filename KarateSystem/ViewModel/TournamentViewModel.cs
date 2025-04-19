using FontAwesome.Sharp;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
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
    public class TournamentViewModel : ViewModelBase
    {
        #region Fields
        private string _searchTextTour;
        private string _searchTextTourComp;
        private bool _isEditingExisting;
        private TournamentDto _selectedTour;
        private TournamentDto _editingTour;
        private StatusOption _selectedStatus;
        private TourCompetitorDto _selectedCompetitorTour;
        private TourCompetitorDto _selectedCatKataTour;
        private TourCompetitorDto _selectedCatKumiteTour;

        private ObservableCollection<TournamentDto> _tournaments;
        private List<TournamentDto> _allTournaments;
        private ObservableCollection<TourCompetitorDto> _allCompetitorsTour;
        private ObservableCollection<TourCompetitorDto> _competitorsTour;
        //private ObservableCollection<TourCatKataDto> _catKataTour;
        //private ObservableCollection<TourCatKumiteDto> _catKumiteTour;
        public ObservableCollection<StatusOption> StatusOptions { get; set; } = new(StatusOptionsList);

        private readonly ITournamentRepository _tournamentRepository;
        //private readonly ICompetitorRepository _competitorRepository;
        private readonly ISearchService _searchService;
        #endregion

        #region Commands
        public ICommand AddTourCommand { get; }
        public ICommand EditTourCommand { get; }
        public ICommand CancelTourCommand { get; }
        public ICommand UpdateTourCommand { get; }
        #endregion

        #region Properties
        public TourCompetitorDto SelectedCatKataTour
        {
            get => _selectedCatKataTour;
            set
            {
                _selectedCatKataTour = value;
                OnPropertyChanged(nameof(SelectedCatKataTour));
            }
        }
        public TourCompetitorDto SelectedCatKumiteTour
        {
            get => _selectedCatKumiteTour;
            set
            {
                _selectedCatKumiteTour = value;
                OnPropertyChanged(nameof(SelectedCatKumiteTour));
            }
        }
        public string SearchTextTourComp
        {
            get => _searchTextTourComp;
            set
            {
                _searchTextTourComp = value;
                OnPropertyChanged(nameof(SearchTextTourComp));
                FilterCompTour();
            }
        }
        public ObservableCollection<TourCompetitorDto> AllCompetitorsTour
        {
            get => _allCompetitorsTour;
            set
            {
                _allCompetitorsTour = value;
                OnPropertyChanged(nameof(AllCompetitorsTour));
            }
        }
        public ObservableCollection<TourCompetitorDto> CompetitorsTour
        {
            get => _competitorsTour;
            set
            {
                _competitorsTour = value;
                OnPropertyChanged(nameof(CompetitorsTour));
            }
        }
        public TourCompetitorDto SelectedCompetitorTour
        {
            get => _selectedCompetitorTour;
            set
            {
                _selectedCompetitorTour = value;
                OnPropertyChanged(nameof(SelectedCompetitorTour));
            }
        }
        public StatusOption SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }
        public string SearchTextTour
        {
            get => _searchTextTour;
            set
            {
                _searchTextTour = value;
                OnPropertyChanged(nameof(SearchTextTour));
                FilterTour();
            }
        }
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

        public TournamentDto SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }
        public TournamentDto EditingTour
        {
            get => _editingTour;
            set
            {
                _editingTour = value;
                OnPropertyChanged(nameof(EditingTour));
                OnPropertyChanged(nameof(EditingTour.TourCompetitors));
            }
        }
        public ObservableCollection<TournamentDto> Tournaments
        {
            get => _tournaments;
            set
            {
                _tournaments = value;
                OnPropertyChanged(nameof(Tournaments));
            }
        }

        #endregion

        public TournamentViewModel(ITournamentRepository tournamentRepository,
            ISearchService searchService)
        {
            _tournamentRepository = tournamentRepository;
            _searchService = searchService;

            EditTourCommand = new ViewModelCommand(ExecuteEditTourCommand, CanEditTour);
            CancelTourCommand = new ViewModelCommand(ExecuteCancelTourCommand);
            UpdateTourCommand = new ViewModelCommand(ExecuteUpdateTourCommand, CanCancelTour);
            AddTourCommand = new ViewModelCommand(ExecuteAddTourCommand);

            LoadAsync();
        }

        public async void LoadAsync()
        {
            _allTournaments = await _tournamentRepository.GetAllTournamentsAsync();
            Tournaments = new ObservableCollection<TournamentDto>(_allTournaments);

            EditingTour = new TournamentDto
            {
                TourDate = DateTime.Now,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>()
            };

            _allCompetitorsTour = new ObservableCollection<TourCompetitorDto>();
            CompetitorsTour = _allCompetitorsTour;

            //CatKataTour = new ObservableCollection<TourCompetitorDto>();
            //CatKumiteTour = new ObservableCollection<TourCompetitorDto>();
        }

        #region TournamentDetails
        private bool CanEditTour(object obj) => SelectedTour != null;
        private bool CanCancelTour(object obj) => SelectedTour != null && EditingTour != null;

        private void FilterTour()
        {
            Tournaments = string.IsNullOrEmpty(SearchTextTour)
                ? new ObservableCollection<TournamentDto>(_allTournaments)
                : new ObservableCollection<TournamentDto>(
                    _searchService.SearchInCollection(_allTournaments, SearchTextTour, "TourName", "TourPlace"));
        }

        private void ExecuteEditTourCommand(object obj)
        {
            if (SelectedTour == null) return;

            EditingTour = new TournamentDto
            {
                TourId = SelectedTour.TourId,
                TourName = SelectedTour.TourName,
                TourPlace = SelectedTour.TourPlace,
                TourDate = SelectedTour.TourDate,
                Status = SelectedTour.Status,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>(
                    SelectedTour.TourCompetitors ?? new List<TourCompetitorDto>())
            };
            SelectedStatus = StatusOptions.FirstOrDefault(opt => opt.Value == EditingTour.Status);
            _allCompetitorsTour = new ObservableCollection<TourCompetitorDto>(SelectedTour.TourCompetitors);
            CompetitorsTour = _allCompetitorsTour;
            //CatKataTour = new ObservableCollection<TourCompetitorDto>(SelectedTour.TourCatKatas);
            //CatKumiteTour = new ObservableCollection<TourCompetitorDto>(SelectedTour.TourCatKumites);
            IsEditingExisting = true;
        }
        private void ExecuteCancelTourCommand(object obj)
        {
            EditingTour = new TournamentDto
            {
                TourDate = DateTime.Now,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>()
            };
            SelectedTour = null;
            SelectedStatus = null;
            IsEditingExisting = false;
        }
        private async void ExecuteUpdateTourCommand(object obj)
        {
            try
            {
                if (!IsTourValid(EditingTour) || SelectedTour == null) return;
               
                SelectedTour.TourId = EditingTour.TourId;
                SelectedTour.TourName = EditingTour.TourName;
                SelectedTour.TourPlace = EditingTour.TourPlace;
                SelectedTour.TourDate = EditingTour.TourDate;
                SelectedTour.Status = SelectedStatus.Value;
                SelectedTour.TourCompetitors = EditingTour.TourCompetitors;

                await _tournamentRepository.UpdateTournamentAsync(SelectedTour);

                Tournaments = new ObservableCollection<TournamentDto>(await _tournamentRepository.GetAllTournamentsAsync());
                ExecuteCancelTourCommand(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji turnieju: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteAddTourCommand(object obj)
        {
            try
            {
                if (!IsTourValid(EditingTour)) return;

                EditingTour.Status = SelectedStatus.Value;

                await _tournamentRepository.AddTourAsync(EditingTour);

                _allTournaments = await _tournamentRepository.GetAllTournamentsAsync();
                Tournaments = new ObservableCollection<TournamentDto>(_allTournaments);
                ExecuteCancelTourCommand(null);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania turnieju: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsTourValid(TournamentDto tournament)
        {
            if (string.IsNullOrWhiteSpace(tournament.TourName) ||
                string.IsNullOrWhiteSpace(tournament.TourPlace) ||
                tournament.Status < 0)
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.\n", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        #endregion

        #region Competitors
        private void FilterCompTour()
        {
            CompetitorsTour = string.IsNullOrWhiteSpace(SearchTextTourComp)
                ? new ObservableCollection<TourCompetitorDto>(_allCompetitorsTour)
                : new ObservableCollection<TourCompetitorDto>(
                    _searchService.SearchInCollection(_allCompetitorsTour, SearchTextTourComp, "CompFirstName", "CompLastName"));
        }
        #endregion
    }
}
