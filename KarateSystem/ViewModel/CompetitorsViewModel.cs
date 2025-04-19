﻿using Accessibility;
using AutoMapper;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Enum = KarateSystem.Misc.Enum;

namespace KarateSystem.ViewModel
{
    public class CompetitorsViewModel : ViewModelBase
    {
        #region Fields
        private string _searchTextComp;
        private bool _isEditingExisting;
        private CompetitorDto _selectedComp;
        private CompetitorDto _editingComp;
        private Enum.Gender _selectedGenderComp; 
        private string _selectedFilterType;
        private string _selectedFilterValue;
        private TourCompetitorDto _selectedTourComp;

        private ObservableCollection<CompetitorDto> _competitors;
        private ObservableCollection<DegreeDto> _degrees;
        private ObservableCollection<ClubDto> _clubs;
        private List<CompetitorDto> _allCompetitors;
        public ObservableCollection<string> FilterTypes { get; set; } = new() { "", "Płeć", "Stopień", "Klub" };
        public ObservableCollection<string> FilterValues { get; set; } = new();
        public ObservableCollection<Enum.Gender> GenderOption { get; set; } =
        new ObservableCollection<Enum.Gender>((Enum.Gender[])System.Enum.GetValues(typeof(Enum.Gender)));

        private readonly ICompetitorRepository _competitorRepository;
        private readonly IDegreeRepository _degreeRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ISearchService _searchService;
        #endregion

        #region Commands
        public ICommand EditCompCommand { get; }
        public ICommand CancelCompCommand { get; }
        public ICommand UpdateCompCommand { get; }
        public ICommand AddCompCommand { get; }
        public ICommand FilterCommand { get; }
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
        public ObservableCollection<CompetitorDto> Competitors
        {
            get => _competitors;
            set
            {
                _competitors = value;
                OnPropertyChanged(nameof(Competitors));
            }
        }
        public ObservableCollection<ClubDto> Clubs
        {
            get => _clubs;
            set
            {
                _clubs = value;
                OnPropertyChanged(nameof(Clubs));
            }
        }
        public ObservableCollection<DegreeDto> Degrees
        {
            get => _degrees;
            set
            {
                _degrees = value;
                OnPropertyChanged(nameof(Degrees));
            }
        }
        public CompetitorDto SelectedCompetitor
        {
            get => _selectedComp;
            set
            {
                _selectedComp = value;
                OnPropertyChanged(nameof(SelectedCompetitor));
            }
        }
        public TourCompetitorDto SelectedTourComp
        {
            get => _selectedTourComp;
            set
            {
                _selectedTourComp = value;
                OnPropertyChanged(nameof(SelectedTourComp));
                OnPropertyChanged(nameof(SelectedTourComp.KataCatName));
                OnPropertyChanged(nameof(SelectedTourComp.KumiteCatName));
            }
        }
        public CompetitorDto EditingComp
        {
            get => _editingComp;
            set
            {
                _editingComp = value;
                OnPropertyChanged(nameof(EditingComp));
            }
        }
        public Enum.Gender SelectedGenderComp
        {
            get => _selectedGenderComp;
            set
            {
                _selectedGenderComp = value;
                OnPropertyChanged(nameof(SelectedGenderComp));
            }
        }
        public string SearchTextComp
        {
            get => _searchTextComp;
            set
            {
                _searchTextComp = value;
                OnPropertyChanged(nameof(SearchTextComp));
                FilterComp();
            }
        }
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
        #endregion
        public CompetitorsViewModel(ICompetitorRepository competitorRepository,
            ISearchService searchService,
            IDegreeRepository degreeRepository,
            IClubRepository clubRepository)
        {
            _competitorRepository = competitorRepository;
            _searchService = searchService;
            _degreeRepository = degreeRepository;
            _clubRepository = clubRepository;

            EditCompCommand = new ViewModelCommand(ExecuteEditCompetitorCommand, CanEditCompetitor);
            CancelCompCommand = new ViewModelCommand(ExecuteCancelCompCommand);
            UpdateCompCommand = new ViewModelCommand(ExecuteUpdateCompCommand, CanCancelCompetitor);
            AddCompCommand = new ViewModelCommand(ExecuteAddCompCommand);
            FilterCommand = new ViewModelCommand(ExecuteFilter);

            LoadAsync();
        }
        private bool CanEditCompetitor(object obj) => SelectedCompetitor != null;
        private bool CanCancelCompetitor(object obj) => SelectedCompetitor != null && EditingComp != null;
        private async void LoadAsync()
        {
            _allCompetitors = await _competitorRepository.GetAllCompetitorsAsync();
            Competitors = new ObservableCollection<CompetitorDto>(_allCompetitors);

            var degrees = await _degreeRepository.GetAllDegreeAsync();
            Degrees = new ObservableCollection<DegreeDto>(degrees);

            var clubs = await _clubRepository.GetAllClubsAsync();
            Clubs = new ObservableCollection<ClubDto>(clubs);


            EditingComp = new CompetitorDto
            {
                CompDateOfBirth = new DateTime(2000, 1, 1),
                TourCompetitors = new ObservableCollection<TourCompetitorDto>()
            };
        }
        private void FilterComp()
        {
            Competitors = string.IsNullOrWhiteSpace(SearchTextComp)
                ? new ObservableCollection<CompetitorDto>(_allCompetitors)
                : new ObservableCollection<CompetitorDto>(
                    _searchService.SearchInCollection(_allCompetitors, SearchTextComp, "CompFirstName", "CompLastName"));
        }
        private void ExecuteFilter(object obj)
        {
            if (string.IsNullOrEmpty(SelectedFilterType) || string.IsNullOrEmpty(SelectedFilterValue) || SelectedFilterType == "")
            {
                Competitors = new ObservableCollection<CompetitorDto>(_allCompetitors);
                return;
            }

            IEnumerable<CompetitorDto> filtered = _allCompetitors;

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

            Competitors = new ObservableCollection<CompetitorDto>(filtered);
        }

        private void ExecuteEditCompetitorCommand(object obj)
        {
            if (SelectedCompetitor == null) return;

            EditingComp = new CompetitorDto
            {
                CompId = SelectedCompetitor.CompId,
                CompFirstName = SelectedCompetitor.CompFirstName,
                CompLastName = SelectedCompetitor.CompLastName,
                CompDateOfBirth = SelectedCompetitor.CompDateOfBirth,
                CompGender = SelectedCompetitor.CompGender,
                CompWeight = SelectedCompetitor.CompWeight,
                CompDegreeId = SelectedCompetitor.CompDegreeId,
                CompClubId = SelectedCompetitor.CompClubId,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>(
                SelectedCompetitor.TourCompetitors ?? new List<TourCompetitorDto>())
            };
            SelectedGenderComp = EditingComp.CompGender ? Enum.Gender.Mężczyzna : Enum.Gender.Kobieta;
            IsEditingExisting = true;
        }
        private void ExecuteCancelCompCommand(object obj)
        {
            EditingComp = new CompetitorDto
            {
                CompDateOfBirth = new DateTime(2000, 1, 1),
                TourCompetitors = new ObservableCollection<TourCompetitorDto>()
            };
            IsEditingExisting = false;
            SelectedCompetitor = null;
            SelectedGenderComp = 0;
        }

        private async void ExecuteUpdateCompCommand(object obj)
        {
            try
            {
                if (!IsCompetitorValid(EditingComp) || SelectedCompetitor == null) return;

                SelectedCompetitor.CompFirstName = EditingComp.CompFirstName;
                SelectedCompetitor.CompLastName = EditingComp.CompLastName;
                SelectedCompetitor.CompDateOfBirth = EditingComp.CompDateOfBirth;
                SelectedCompetitor.CompGender = SelectedGenderComp == Enum.Gender.Mężczyzna; 
                SelectedCompetitor.CompWeight = EditingComp.CompWeight;
                SelectedCompetitor.CompDegreeId = EditingComp.CompDegreeId;
                SelectedCompetitor.CompClubId = EditingComp.CompClubId;

                await _competitorRepository.UpdateCompAsync(SelectedCompetitor);

                Competitors = new ObservableCollection<CompetitorDto>(await _competitorRepository.GetAllCompetitorsAsync());
                ExecuteCancelCompCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji zawodnika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteAddCompCommand(object obj)
        {
            try
            {
                if (!IsCompetitorValid(EditingComp)) return;

                EditingComp.CompGender = SelectedGenderComp == Enum.Gender.Mężczyzna;

                await _competitorRepository.AddCompAsync(EditingComp);
                
                Competitors = new ObservableCollection<CompetitorDto>(await _competitorRepository.GetAllCompetitorsAsync());
                ExecuteCancelCompCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania zawodnika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    foreach (var degree in _allCompetitors.Select(c => c.DegreeName).Distinct())
                        FilterValues.Add(degree);
                    break;

                case "Klub":
                    foreach (var club in _allCompetitors.Select(c => c.ClubName).Distinct())
                        FilterValues.Add(club);
                    break;
            }
        }
        private bool IsCompetitorValid(CompetitorDto competitor)
        {
            if (string.IsNullOrWhiteSpace(competitor.CompFirstName) ||
                string.IsNullOrWhiteSpace(competitor.CompLastName) ||
                competitor.CompWeight < 0 ||
                competitor.CompDegreeId <= 0 ||
                competitor.CompClubId <= 0)
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.\n", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
