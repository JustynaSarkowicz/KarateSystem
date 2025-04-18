﻿using FontAwesome.Sharp;
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
        private string _selectedFilterType;
        private string _selectedFilterValue;
        private TournamentDto _selectedTour;
        private TournamentDto _editingTour;
        private StatusOption _selectedStatus;
        private TourCompetitorDto _selectedCompetitorTour;
        private TourCatKataDto _selectedCatKataTour;
        private TourCatKumiteDto _selectedCatKumiteTour;

        private ObservableCollection<TournamentDto> _tournaments;
        private List<TournamentDto> _allTournaments;
        private ObservableCollection<TourCompetitorDto> _allCompetitorsTour;
        private ObservableCollection<TourCompetitorDto> _competitorsTour;
        private ObservableCollection<TourCatKataDto> _catKataTour;
        private ObservableCollection<TourCatKumiteDto> _catKumiteTour;
        public ObservableCollection<StatusOption> StatusOptions { get; set; } = new(StatusOptionsList);
        public ObservableCollection<string> FilterTypes { get; set; } = new() { "", "Płeć", "Stopień", "Klub" };
        public ObservableCollection<string> FilterValues { get; set; } = new();

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITourCompetitorRepository _tourCompetitorRepository;
        private readonly ISearchService _searchService;
        private readonly ITourCatKumiteRepository _tourCatKumiteRepository;
        private readonly ITourCatKataRepository _tourCatKataRepository;
        #endregion

        #region Commands
        public ICommand AddTourCommand { get; }
        public ICommand EditTourCommand { get; }
        public ICommand CancelTourCommand { get; }
        public ICommand UpdateTourCommand { get; }
        public ICommand FilterCompCommand { get; }
        public ICommand DeleteCompFromTourCommand { get; }
        public ICommand DeleteCatKataFromTourCommand { get; }
        public ICommand DeleteCatKumiteFromTourCommand { get; }
        #endregion

        #region Properties
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set
            {
                _selectedFilterType = value;
                OnPropertyChanged(nameof(SelectedFilterType));
                OnFilterTypeChanged();
            }
        }
        public string SelectedFilterValue
        {
            get => _selectedFilterValue;
            set
            {
                _selectedFilterValue = value;
                OnPropertyChanged(nameof(SelectedFilterValue));
            }
        }
        public ObservableCollection<TourCatKataDto> CatKataTour
        {
            get => _catKataTour;
            set
            {
                _catKataTour = value;
                OnPropertyChanged(nameof(CatKataTour));
            }
        }
        public ObservableCollection<TourCatKumiteDto> CatKumiteTour
        {
            get => _catKumiteTour;
            set
            {
                _catKumiteTour = value;
                OnPropertyChanged(nameof(CatKumiteTour));
            }
        }
        public TourCatKataDto SelectedCatKataTour
        {
            get => _selectedCatKataTour;
            set
            {
                _selectedCatKataTour = value;
                OnPropertyChanged(nameof(SelectedCatKataTour));
            }
        }
        public TourCatKumiteDto SelectedCatKumiteTour
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
            ISearchService searchService,
            ITourCompetitorRepository tourCompetitorRepository,
            ITourCatKumiteRepository tourCatKumiteRepository,
            ITourCatKataRepository tourCatKataRepository)
        {
            _tournamentRepository = tournamentRepository;
            _searchService = searchService;
            _tourCompetitorRepository = tourCompetitorRepository;
            _tourCatKumiteRepository = tourCatKumiteRepository;
            _tourCatKataRepository = tourCatKataRepository;

            EditTourCommand = new ViewModelCommand(ExecuteEditTourCommand, CanEditTour);
            CancelTourCommand = new ViewModelCommand(ExecuteCancelTourCommand);
            UpdateTourCommand = new ViewModelCommand(ExecuteUpdateTourCommand, CanCancelTour);
            AddTourCommand = new ViewModelCommand(ExecuteAddTourCommand);

            FilterCompCommand = new ViewModelCommand(ExecuteCompFilter);
            DeleteCompFromTourCommand = new ViewModelCommand(ExecuteDeleteCompFromTourCommand, CanDeleteComp);

            DeleteCatKataFromTourCommand = new ViewModelCommand(ExecuteDeleteCatKataFromTourCommand, CanDeleteCatKata);
            DeleteCatKumiteFromTourCommand = new ViewModelCommand(ExecuteDeleteCatKumiteFromTourCommand, CanDeleteCatKumite);

            LoadAsync();
        }

        public async void LoadAsync()
        {
            _allTournaments = await _tournamentRepository.GetAllTournamentsAsync();
            Tournaments = new ObservableCollection<TournamentDto>(_allTournaments);

            EditingTour = new TournamentDto
            {
                TourDate = DateTime.Now,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>(),
                TourCatKatas = new ObservableCollection<TourCatKataDto>(),
                TourCatKumites = new ObservableCollection<TourCatKumiteDto>()
            };
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

        private async void ExecuteEditTourCommand(object obj)
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
                    SelectedTour.TourCompetitors ?? new List<TourCompetitorDto>()),
                TourCatKatas = new ObservableCollection<TourCatKataDto>(
                    SelectedTour.TourCatKatas ?? new List<TourCatKataDto>()),
                TourCatKumites = new ObservableCollection<TourCatKumiteDto>(
                    SelectedTour.TourCatKumites ?? new List<TourCatKumiteDto>())
            };
            SelectedStatus = StatusOptions.FirstOrDefault(opt => opt.Value == EditingTour.Status);

            _allCompetitorsTour = new ObservableCollection<TourCompetitorDto>(
                await _tourCompetitorRepository.GetTourCompetitorsByIdTourAsync(SelectedTour.TourId));
            CompetitorsTour = _allCompetitorsTour;

            CatKataTour = new ObservableCollection<TourCatKataDto>(
                await _tourCatKataRepository.GetCatKataByIdTourAsync(SelectedTour.TourId));

            CatKumiteTour = new ObservableCollection<TourCatKumiteDto>(
                await _tourCatKumiteRepository.GetCatKumiteByIdTourAsync(SelectedTour.TourId));

            IsEditingExisting = true;
        }
        private void ExecuteCancelTourCommand(object obj)
        {
            EditingTour = new TournamentDto
            {
                TourDate = DateTime.Now,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>(),
                TourCatKatas = new ObservableCollection<TourCatKataDto>(),
                TourCatKumites = new ObservableCollection<TourCatKumiteDto>()
            };
            CompetitorsTour = new ObservableCollection<TourCompetitorDto>();
            CatKataTour = new ObservableCollection<TourCatKataDto>();
            CatKumiteTour = new ObservableCollection<TourCatKumiteDto>();
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
        private bool CanDeleteComp(object obj) => SelectedTour != null && SelectedCompetitorTour != null;
        private async void ExecuteDeleteCompFromTourCommand(object obj)
        {
            if (SelectedCompetitorTour == null) return;
            try
            {
                await _tourCompetitorRepository.DeleteTourComp(SelectedCompetitorTour.TourCompId);
                CompetitorsTour = new ObservableCollection<TourCompetitorDto>(
                    await _tourCompetitorRepository.GetTourCompetitorsByIdTourAsync(SelectedTour.TourId));
                SelectedCompetitorTour = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania zawodnika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FilterCompTour()
        {
            CompetitorsTour = string.IsNullOrWhiteSpace(SearchTextTourComp)
                ? new ObservableCollection<TourCompetitorDto>(_allCompetitorsTour)
                : new ObservableCollection<TourCompetitorDto>(
                    _searchService.SearchInCollection(_allCompetitorsTour, SearchTextTourComp, "CompFirstName", "CompLastName"));
        }
        private void OnFilterTypeChanged()
        {
            FilterValues.Clear();

            switch (SelectedFilterType)
            {
                case "Płeć":
                    FilterValues.Add("Mężczyzna");
                    FilterValues.Add("Kobieta");
                    break;

                case "Stopień":
                    foreach (var degree in _allCompetitorsTour.Select(c => c.DegreeName).Distinct())
                        FilterValues.Add(degree);
                    break;

                case "Klub":
                    foreach (var club in _allCompetitorsTour.Select(c => c.ClubName).Distinct())
                        FilterValues.Add(club);
                    break;
            }
        }
        private void ExecuteCompFilter(object obj)
        {
            if (string.IsNullOrEmpty(SelectedFilterType) || string.IsNullOrEmpty(SelectedFilterValue) || SelectedFilterType == "")
            {
                CompetitorsTour = new ObservableCollection<TourCompetitorDto>(_allCompetitorsTour);
                return;
            }

            IEnumerable<TourCompetitorDto> filtered = _allCompetitorsTour;

            switch (SelectedFilterType)
            {
                case "Płeć":
                    bool isMale = SelectedFilterValue == "Mężczyzna";
                    filtered = filtered.Where(c => c.CompGender == isMale);
                    break;

                case "Stopień":
                    filtered = filtered.Where(c => c.DegreeName == SelectedFilterValue);
                    break;

                case "Klub":
                    filtered = filtered.Where(c => c.ClubName == SelectedFilterValue);
                    break;
            }

            CompetitorsTour = new ObservableCollection<TourCompetitorDto>(filtered);
        }
        #endregion

        #region Category Kata
        private bool CanDeleteCatKata(object obj) => SelectedTour != null && SelectedCatKataTour != null;
        private async void ExecuteDeleteCatKataFromTourCommand(object obj)
        {
            if (SelectedCatKataTour == null) return;
            try
            {
                await _tourCatKataRepository.DeleteCatKataFromTour(SelectedCatKataTour.TourCatKataId);
                CatKataTour = new ObservableCollection<TourCatKataDto>(
                    await _tourCatKataRepository.GetCatKataByIdTourAsync(SelectedTour.TourId));
                SelectedCatKataTour = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Category Kumite
        private bool CanDeleteCatKumite(object obj) => SelectedTour != null && SelectedCatKumiteTour != null;
        private async void ExecuteDeleteCatKumiteFromTourCommand(object obj)
        {
            if (SelectedCatKumiteTour == null) return;
            try
            {
                await _tourCatKumiteRepository.DeleteCatKumiteFromTour(SelectedCatKumiteTour.TourCatKumiteId);
                CatKumiteTour = new ObservableCollection<TourCatKumiteDto>(
                    await _tourCatKumiteRepository.GetCatKumiteByIdTourAsync(SelectedTour.TourId));
                SelectedCatKumiteTour = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania kategorii kumite: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
