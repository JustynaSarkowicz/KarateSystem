using Accessibility;
using AutoMapper;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static KarateSystem.Misc.Enum;
using static KarateSystem.Misc.Helper;

namespace KarateSystem.ViewModel
{
    public class CompetitorsViewModel : ViewModelBase
    {
        #region Fields
        private string _searchTextComp;
        private bool _isEditingExisting;
        private CompetitorDto _selectedComp;
        private CompetitorDto _editingComp;
        private GenderOption2 _selectedGenderComp; 
        private string _selectedFilterType;
        private string _selectedFilterValue;
        private TourCompetitorDto _selectedTourComp;

        private ObservableCollection<CompetitorDto> _competitors;
        private ObservableCollection<DegreeDto> _degrees;
        private ObservableCollection<ClubDto> _clubs;
        private List<CompetitorDto> _allCompetitors;
        public ObservableCollection<string> FilterTypes { get; set; } = new() { "", "Płeć", "Stopień", "Klub" };
        public ObservableCollection<string> FilterValues { get; set; } = new();

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
        public GenderOption2 SelectedGenderComp
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

            _ = LoadAsync();
        }
        private bool CanEditCompetitor(object obj) => SelectedCompetitor != null;
        private bool CanCancelCompetitor(object obj) => SelectedCompetitor != null && EditingComp != null;
        private async Task LoadAsync()
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
            SelectedGenderComp = GenderOptions2.FirstOrDefault(g => g.Value == EditingComp.CompGender);
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
            SelectedGenderComp = null;
        }

        private async void ExecuteUpdateCompCommand(object obj)
        {
            if (SelectedCompetitor == null || EditCompCommand == null) return;

            if (!Competitors.Contains(SelectedCompetitor)) return;
            
            SelectedCompetitor.CompFirstName = EditingComp.CompFirstName;
            SelectedCompetitor.CompLastName = EditingComp.CompLastName;
            SelectedCompetitor.CompDateOfBirth = EditingComp.CompDateOfBirth;
            SelectedCompetitor.CompGender = SelectedGenderComp?.Value ?? false;
            SelectedCompetitor.CompWeight = EditingComp.CompWeight;
            SelectedCompetitor.CompDegreeId = EditingComp.CompDegreeId;
            SelectedCompetitor.CompClubId = EditingComp.CompClubId;
            var result = await _competitorRepository.UpdateCompAsync(SelectedCompetitor);
            if(!result)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ExecuteCancelCompCommand(obj);
            _allCompetitors = await _competitorRepository.GetAllCompetitorsAsync();
            Competitors = new ObservableCollection<CompetitorDto>(_allCompetitors);
        }
        private async void ExecuteAddCompCommand(object obj)
        {
            if(EditingComp == null) return;
            EditingComp.CompGender = SelectedGenderComp?.Value ?? false;
            var result = await _competitorRepository.AddCompAsync(EditingComp);
            if (!result)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _allCompetitors = await _competitorRepository.GetAllCompetitorsAsync();
            Competitors = new ObservableCollection<CompetitorDto>(_allCompetitors);

            ExecuteCancelCompCommand(obj);
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
    }
}
