using FontAwesome.Sharp;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using KarateSystem.Views;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
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
        private KataCategoryDto _selectedKataCategory;
        private KumiteCategoryDto _selectedKumiteCategory;
        private MatDto _selectedKataCategoryMat;
        private MatDto _selectedKumiteCategoryMat;

        private TournamentDto _selectedTourToSetCompToCat;
        private ObservableCollection<TourCatKataDto> _tourCatKatas;
        private ObservableCollection<TourCatKumiteDto> _tourCatKumites;
        private TourCatKataDto _selectedTourCatKata;
        private TourCatKumiteDto _selectedTourCatKumite;
        private ObservableCollection<TourCompetitorDto> _compTourCatKata;
        private ObservableCollection<TourCompetitorDto> _compTourCatKumite;
        private TourCompetitorDto _selectedCompTourCatKata;
        private TourCompetitorDto _selectedCompTourCatKumite;

        private ObservableCollection<TournamentDto> _tournaments;
        private List<TournamentDto> _allTournaments;
        private ObservableCollection<TourCompetitorDto> _allCompetitorsTour;
        private ObservableCollection<TourCompetitorDto> _competitorsTour;
        private ObservableCollection<TourCatKataDto> _catKataTour;
        private ObservableCollection<TourCatKumiteDto> _catKumiteTour;
        private ObservableCollection<KataCategoryDto> _kataCategory;
        private ObservableCollection<KumiteCategoryDto> _kumiteCategory;
        private ObservableCollection<MatDto> _mats;

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITourCompetitorRepository _tourCompetitorRepository;
        private readonly ISearchService _searchService;
        private readonly ITourCatKumiteRepository _tourCatKumiteRepository;
        private readonly ITourCatKataRepository _tourCatKataRepository;
        private readonly IKataCategoryRepository _kataCategoryRepository;
        private readonly IKumiteCategoryRepository _kumiteCategoryRepository;
        private readonly IMatRepository _matRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFightRepository _fightRepository;
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
        public ICommand AddCatKataFromTourCommand { get; }
        public ICommand AddCatKumiteFromTourCommand { get; }
        public ICommand AddCompToTourCommand { get; }
        public ICommand ChoseTourToSetCompToCatCommand { get; }
        public ICommand SetCompToTourCatKataCommand { get;  }
        public ICommand SetCompToTourCatKumiteCommand { get;  }
        public ICommand DeleteCompFromTourCatKataCommand { get;  }
        public ICommand DeleteCompFromTourCatKumiteCommand { get;  }
        public ICommand SetCompAutomaticCatKata { get; }
        public ICommand SetCompAutomaticCatKumite { get; }
        public ICommand SetFights { get; }
        #endregion

        #region Properties
        public ObservableCollection<string> FilterTypes { get; set; } = new() { "", "Płeć", "Stopień", "Klub" };
        public ObservableCollection<string> FilterValues { get; set; } = new();
        public TourCompetitorDto SelectedCompTourCatKata
        {
            get => _selectedCompTourCatKata;
            set
            {
                _selectedCompTourCatKata = value;
                OnPropertyChanged(nameof(SelectedCompTourCatKata));
            }
        }
        public TourCompetitorDto SelectedCompTourCatKumite
        {
            get => _selectedCompTourCatKumite;
            set
            {
                _selectedCompTourCatKumite = value;
                OnPropertyChanged(nameof(SelectedCompTourCatKumite));
            }
        }
        public ObservableCollection<TourCompetitorDto> CompTourCatKata
        {
            get => _compTourCatKata;
            set
            {
                _compTourCatKata = value;
                OnPropertyChanged(nameof(CompTourCatKata));
            }
        }
        public ObservableCollection<TourCompetitorDto> CompTourCatKumite
        {
            get => _compTourCatKumite;
            set
            {
                _compTourCatKumite = value;
                OnPropertyChanged(nameof(CompTourCatKumite));
            }
        }
        public TourCatKumiteDto SelectedTourCatKumite
        {
            get => _selectedTourCatKumite;
            set
            {
                _selectedTourCatKumite = value;
                OnPropertyChanged(nameof(SelectedTourCatKumite));
                LoadCompetitorsForKumiteCategoryAsync();
            }
        }
        public TourCatKataDto SelectedTourCatKata
        {
            get => _selectedTourCatKata;
            set
            {
                _selectedTourCatKata = value;
                OnPropertyChanged(nameof(SelectedTourCatKata));
                LoadCompetitorsForKataCategoryAsync();
            }
        }
        public ObservableCollection<TourCatKataDto> TourCatKatas
        {
            get => _tourCatKatas;
            set
            {
                _tourCatKatas = value;
                OnPropertyChanged(nameof(TourCatKatas));
            }
        }
        public ObservableCollection<TourCatKumiteDto> TourCatKumites
        {
            get => _tourCatKumites;
            set
            {
                _tourCatKumites = value;
                OnPropertyChanged(nameof(TourCatKumites));
            }
        }
        public TournamentDto SelectedTourToSetCompToCat
        {
            get => _selectedTourToSetCompToCat;
            set
            {
                _selectedTourToSetCompToCat = value;
                OnPropertyChanged(nameof(SelectedTourToSetCompToCat));
            }
        }
        public MatDto SelectedKataCategoryMat
        {
            get => _selectedKataCategoryMat;
            set
            {
                _selectedKataCategoryMat = value;
                OnPropertyChanged(nameof(SelectedKataCategoryMat));
            }
        }
        public MatDto SelectedKumiteCategoryMat
        {
            get => _selectedKumiteCategoryMat;
            set
            {
                _selectedKumiteCategoryMat = value;
                OnPropertyChanged(nameof(SelectedKumiteCategoryMat));
            }
        }
        public ObservableCollection<MatDto> Mats
        {
            get => _mats;
            set
            {
                _mats = value;
                OnPropertyChanged(nameof(Mats));
            }
        }
        public KataCategoryDto SelectedKataCategory
        {
            get => _selectedKataCategory;
            set
            {
                _selectedKataCategory = value;
                OnPropertyChanged(nameof(SelectedKataCategory));
            }
        }
        public KumiteCategoryDto SelectedKumiteCategory
        {
            get => _selectedKumiteCategory;
            set
            {
                _selectedKumiteCategory = value;
                OnPropertyChanged(nameof(SelectedKumiteCategory));
            }
        }
        public ObservableCollection<KataCategoryDto> KataCategories
        {
            get => _kataCategory;
            set
            {
                _kataCategory = value;
                OnPropertyChanged(nameof(KataCategories));
            }
        }
        public ObservableCollection<KumiteCategoryDto> KumiteCategories
        {
            get => _kumiteCategory;
            set
            {
                _kumiteCategory = value;
                OnPropertyChanged(nameof(KumiteCategories));
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
            ITourCatKataRepository tourCatKataRepository,
            IKataCategoryRepository kataCategoryRepository,
            IKumiteCategoryRepository kumiteCategoryRepository,
            IFightRepository fightRepository,
            IMatRepository matRepository,
            IServiceProvider serviceProvider)
        {
            _tournamentRepository = tournamentRepository;
            _searchService = searchService;
            _tourCompetitorRepository = tourCompetitorRepository;
            _tourCatKumiteRepository = tourCatKumiteRepository;
            _tourCatKataRepository = tourCatKataRepository;
            _kataCategoryRepository = kataCategoryRepository;
            _kumiteCategoryRepository = kumiteCategoryRepository;
            _matRepository = matRepository;
            _serviceProvider = serviceProvider;
            _fightRepository = fightRepository;

            //First tab
            EditTourCommand = new ViewModelCommand(ExecuteEditTourCommand, CanEditTour);
            CancelTourCommand = new ViewModelCommand(ExecuteCancelTourCommand);
            UpdateTourCommand = new ViewModelCommand(ExecuteUpdateTourCommand, CanCancelTour);
            AddTourCommand = new ViewModelCommand(ExecuteAddTourCommand);

            FilterCompCommand = new ViewModelCommand(ExecuteCompFilter);
            DeleteCompFromTourCommand = new ViewModelCommand(ExecuteDeleteCompFromTourCommand, CanDeleteComp);

            DeleteCatKataFromTourCommand = new ViewModelCommand(ExecuteDeleteCatKataFromTourCommand, CanDeleteCatKata);
            DeleteCatKumiteFromTourCommand = new ViewModelCommand(ExecuteDeleteCatKumiteFromTourCommand, CanDeleteCatKumite);
            AddCatKataFromTourCommand = new ViewModelCommand(ExecuteAddCatKataToTourCommand, CanAddCatKata);
            AddCatKumiteFromTourCommand = new ViewModelCommand(ExecuteAddCatKumiteToTourCommand, CanAddCatKumite);

            AddCompToTourCommand = new ViewModelCommand(ExecuteAddCompToTourCommand);

            //Second tab
            ChoseTourToSetCompToCatCommand = new ViewModelCommand(ExecuteFillCatInTourCommand);
            SetCompToTourCatKataCommand = new ViewModelCommand(ExecuteAddCompToTourCatKataCommand, CanAddCompToTourCatKata);
            SetCompToTourCatKumiteCommand = new ViewModelCommand(ExecuteAddCompToTourCatKumiteCommand, CanAddCompToTourCatKumite);
            DeleteCompFromTourCatKataCommand = new ViewModelCommand(ExecuteDeleteCompFromTourCatKataCommand, CanDeleteCompToTourCatKata);
            DeleteCompFromTourCatKumiteCommand = new ViewModelCommand(ExecuteDeleteCompFromTourCatKumiteCommand, CanDeleteCompToTourCatKumite);
            SetCompAutomaticCatKata = new ViewModelCommand(ExecuteSetCompAutomaticCatKataCommand, CanSetCompAutomatic);
            SetCompAutomaticCatKumite = new ViewModelCommand(ExecuteSetCompAutomaticCatKumiteCommand, CanSetCompAutomatic);
            SetFights = new ViewModelCommand(ExecuteSetFightsCommand);

            WeakEventManager<IKataCategoryRepository, EventArgs>
            .AddHandler(_kataCategoryRepository, nameof(IKataCategoryRepository.KataCatChanged), OnKataCatChanged);
            WeakEventManager<IKumiteCategoryRepository, EventArgs>
            .AddHandler(_kumiteCategoryRepository, nameof(IKumiteCategoryRepository.KumiteCatChanged), OnKumiteCatChanged);
            WeakEventManager<IMatRepository, EventArgs>
            .AddHandler(_matRepository, nameof(IMatRepository.MatsChanged), OnMatChanged);

            _ = LoadAsync();
        }

        public async Task LoadAsync()
        {
            _allTournaments = await _tournamentRepository.GetAllTournamentsAsync().ConfigureAwait(false);
            Tournaments = new ObservableCollection<TournamentDto>(_allTournaments);

            KataCategories = new ObservableCollection<KataCategoryDto>(await _kataCategoryRepository.GetAllKataCategoryAsync());
            KumiteCategories = new ObservableCollection<KumiteCategoryDto>(await _kumiteCategoryRepository.GetAllKumiteCategoryAsync());
            Mats = new ObservableCollection<MatDto>(await _matRepository.GetAllMatAsync());

            EditingTour = new TournamentDto
            {
                TourDate = DateTime.Now,
                TourCompetitors = new ObservableCollection<TourCompetitorDto>(),
                TourCatKatas = new ObservableCollection<TourCatKataDto>(),
                TourCatKumites = new ObservableCollection<TourCatKumiteDto>()
            };
        }

        private async void OnKataCatChanged(object sender, EventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {

                KataCategories = new ObservableCollection<KataCategoryDto>(
                    await _kataCategoryRepository.GetAllKataCategoryAsync());
            });
        }
        private async void OnKumiteCatChanged(object sender, EventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {

                KumiteCategories = new ObservableCollection<KumiteCategoryDto>(
                    await _kumiteCategoryRepository.GetAllKumiteCategoryAsync());
            });
        }
        private async void OnMatChanged(object sender, EventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mats = new ObservableCollection<MatDto>(
                    await _matRepository.GetAllMatAsync());
            });
        }

        #region FirstTab
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
                await _tourCompetitorRepository.GetTourCompetitorsByIdTourAsync(EditingTour.TourId));
            CompetitorsTour = _allCompetitorsTour;

            CatKataTour = new ObservableCollection<TourCatKataDto>(
                await _tourCatKataRepository.GetCatKataByIdTourAsync(EditingTour.TourId));

            CatKumiteTour = new ObservableCollection<TourCatKumiteDto>(
                await _tourCatKumiteRepository.GetCatKumiteByIdTourAsync(EditingTour.TourId));

            IsEditingExisting = true;
        }
        private void ExecuteCancelTourCommand(object obj)
        {
            EditingTour = new TournamentDto
            {
                TourDate = DateTime.Now
            };
            CompetitorsTour = new ObservableCollection<TourCompetitorDto>();
            CatKataTour = new ObservableCollection<TourCatKataDto>();
            CatKumiteTour = new ObservableCollection<TourCatKumiteDto>();
            SelectedTour = null;
            SelectedStatus = null;
            SelectedKumiteCategory = null;
            SelectedKataCategory = null;
            SelectedKataCategoryMat = null;
            SelectedKumiteCategoryMat = null;
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
        private async void ExecuteAddCompToTourCommand(object obj)
        {
            var addCompWindow = _serviceProvider.GetRequiredService<AddCompetitorsView>();
            var viewModel = addCompWindow.DataContext as AddCompetitorsViewModel;

            if (addCompWindow.ShowDialog() == true)
            {
                var selected = viewModel.SelectedCompetitors;

                try
                {
                    foreach (var comp in selected)
                    {
                        if (CompetitorsTour.Any(c => c.CompId == comp.CompId))
                        {
                            MessageBox.Show($"Zawodnik {comp.CompFirstName} {comp.CompLastName} już istnieje w turnieju.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                        var tourComp = new TourCompetitorDto
                        {
                            TourId = EditingTour.TourId,
                            CompId = comp.CompId
                        };
                        await _tourCompetitorRepository.AddCompToTour(tourComp);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas dodawania zawodnika do turnieju: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _allCompetitorsTour = new ObservableCollection<TourCompetitorDto>(
                await _tourCompetitorRepository.GetTourCompetitorsByIdTourAsync(EditingTour.TourId));
            CompetitorsTour = _allCompetitorsTour;
        }

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
        private bool CanDeleteCatKata(object obj) => EditingTour != null && SelectedCatKataTour != null;
        private bool CanAddCatKata(object obj) => EditingTour != null && SelectedKataCategory != null && SelectedKataCategoryMat != null;
        private async void ExecuteDeleteCatKataFromTourCommand(object obj)
        {
            if (SelectedCatKataTour == null || EditingTour == null) return;
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

        private async void ExecuteAddCatKataToTourCommand(object obj)
        {
            if (SelectedKataCategory == null || SelectedKataCategoryMat == null || EditingTour == null) return;
            try
            {
                var newCatKata = new TourCatKataDto
                {
                    TourId = EditingTour.TourId,
                    KataCatId = SelectedKataCategory.KataCatId,
                    MatId = SelectedKataCategoryMat.MatId
                };

                await _tourCatKataRepository.AddCatKataToTour(newCatKata);
                CatKataTour = new ObservableCollection<TourCatKataDto>(
                    await _tourCatKataRepository.GetCatKataByIdTourAsync(EditingTour.TourId));
                SelectedKataCategoryMat = null;
                SelectedKataCategory = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Category Kumite
        private bool CanDeleteCatKumite(object obj) => EditingTour != null && SelectedCatKumiteTour != null;
        private bool CanAddCatKumite(object obj) => EditingTour != null && SelectedKumiteCategory != null && SelectedKumiteCategoryMat != null;
        private async void ExecuteDeleteCatKumiteFromTourCommand(object obj)
        {
            if (SelectedCatKumiteTour == null || EditingTour == null) return;
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
        private async void ExecuteAddCatKumiteToTourCommand(object obj)
        {
            if (SelectedKumiteCategory == null || SelectedKumiteCategoryMat == null || EditingTour == null) return;
            try
            {
                var newCatKumite = new TourCatKumiteDto
                {
                    TourId = EditingTour.TourId,
                    KumiteCatId = SelectedKumiteCategory.KumiteCatId,
                    MatId = SelectedKumiteCategoryMat.MatId
                };
                await _tourCatKumiteRepository.AddCatKumiteToTour(newCatKumite);
                CatKumiteTour = new ObservableCollection<TourCatKumiteDto>(
                    await _tourCatKumiteRepository.GetCatKumiteByIdTourAsync(EditingTour.TourId));
                SelectedKumiteCategoryMat = null;
                SelectedKumiteCategory = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania kategorii kumite: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #endregion

        #region SecondTab
        private bool CanAddCompToTourCatKata(object obj) => SelectedTourToSetCompToCat != null && SelectedTourCatKata != null;
        private bool CanAddCompToTourCatKumite(object obj) => SelectedTourToSetCompToCat != null && SelectedTourCatKumite != null;
        private bool CanDeleteCompToTourCatKata(object obj) => SelectedTourCatKata != null;
        private bool CanDeleteCompToTourCatKumite(object obj) => SelectedTourCatKumite != null;
        private bool CanSetCompAutomatic (object obj) => SelectedTourToSetCompToCat != null;
        private async void ExecuteFillCatInTourCommand(object obj)
        {
            if(SelectedTourToSetCompToCat == null) return;
            TourCatKumites = new ObservableCollection<TourCatKumiteDto>(
                await _tourCatKumiteRepository.GetCatKumiteByIdTourAsync(SelectedTourToSetCompToCat.TourId));
            TourCatKatas = new ObservableCollection<TourCatKataDto>(
                await _tourCatKataRepository.GetCatKataByIdTourAsync(SelectedTourToSetCompToCat.TourId));
        }
        private async void LoadCompetitorsForKataCategoryAsync()
        {
            if (SelectedTourCatKata == null)
            {
                CompTourCatKata = new ObservableCollection<TourCompetitorDto>();
                return;
            }

            var competitorsKata = await _tourCompetitorRepository
                .GetCompetitorToursByCatKataIdAsync(SelectedTourCatKata.TourCatKataId);
            CompTourCatKata = new ObservableCollection<TourCompetitorDto>(competitorsKata);
        }
        private async void LoadCompetitorsForKumiteCategoryAsync()
        {
            if(SelectedTourCatKumite == null)
            {
                CompTourCatKumite = new ObservableCollection<TourCompetitorDto>();
                return;
            }

            var competitorsKumite = await _tourCompetitorRepository
                .GetCompetitorToursByCatKumiteIdAsync(SelectedTourCatKumite.TourCatKumiteId);
            CompTourCatKumite = new ObservableCollection<TourCompetitorDto>(competitorsKumite);
        }

        private async void ExecuteAddCompToTourCatKataCommand(object obj)
        {
            var setCompWindow = _serviceProvider.GetRequiredService<SetCompToCatTourView>();
            var viewModel = setCompWindow.DataContext as SetCompToCatTourViewModel;

            if (viewModel != null && SelectedTourToSetCompToCat != null && SelectedTourCatKata != null)
            {
                viewModel.TourId = SelectedTourToSetCompToCat.TourId;
                await viewModel.LoadAsync();
            }

            var kataCatId = SelectedTourCatKata.TourCatKataId;

            if (setCompWindow.ShowDialog() == true)
            {
                var selected = viewModel.SelectedCompetitors;

                try
                {
                    foreach (var comp in selected)
                    {
                        if (CompTourCatKata.Any(c => c.CompId == comp.CompId && c.TourId == SelectedTourToSetCompToCat.TourId))
                        {
                            MessageBox.Show($"Zawodnik {comp.CompFirstName} {comp.CompLastName} już został dodany do tej kategorii.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                        await _tourCompetitorRepository.AddCompToTourCatKata(comp, kataCatId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas dodawania zawodnika do kategori kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            var competitorsKata = await _tourCompetitorRepository
                .GetCompetitorToursByCatKataIdAsync(SelectedTourCatKata.TourCatKataId);
            CompTourCatKata = new ObservableCollection<TourCompetitorDto>(competitorsKata);
        }

        private async void ExecuteAddCompToTourCatKumiteCommand(object obj)
        {
            var setCompWindow = _serviceProvider.GetRequiredService<SetCompToCatTourView>();
            var viewModel = setCompWindow.DataContext as SetCompToCatTourViewModel;

            if (viewModel != null && SelectedTourToSetCompToCat != null && SelectedTourCatKumite != null)
            {
                viewModel.TourId = SelectedTourToSetCompToCat.TourId;
                await viewModel.LoadAsync();
            }

            var kumiteCatId = SelectedTourCatKumite.TourCatKumiteId;

            if (setCompWindow.ShowDialog() == true)
            {
                var selected = viewModel.SelectedCompetitors;

                try
                {
                    foreach (var comp in selected)
                    {
                        if (CompTourCatKumite.Any(c => c.CompId == comp.CompId && c.TourId == SelectedTourToSetCompToCat.TourId))
                        {
                            MessageBox.Show($"Zawodnik {comp.CompFirstName} {comp.CompLastName} już został dodany do tej kategorii.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }
                        await _tourCompetitorRepository.AddCompToTourCatKumite(comp, kumiteCatId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas dodawania zawodnika do kategori kumite: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            var competitorsKumite = await _tourCompetitorRepository
                .GetCompetitorToursByCatKumiteIdAsync(SelectedTourCatKumite.TourCatKumiteId);
            CompTourCatKumite = new ObservableCollection<TourCompetitorDto>(competitorsKumite);
        }
        private async void ExecuteDeleteCompFromTourCatKataCommand(object obj)
        {
            if (SelectedCompTourCatKata == null) return;
            try
            {
                await _tourCompetitorRepository.DeleteCompFromTourCatKata(SelectedCompTourCatKata);
                var competitorsKata = await _tourCompetitorRepository
                    .GetCompetitorToursByCatKataIdAsync(SelectedTourCatKata.TourCatKataId);
                CompTourCatKata = new ObservableCollection<TourCompetitorDto>(competitorsKata);
                SelectedCompTourCatKata = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania zawodnika z kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteDeleteCompFromTourCatKumiteCommand(object obj)
        {
            if (SelectedCompTourCatKumite == null) return;
            try
            {
                await _tourCompetitorRepository.DeleteCompFromTourCatKumite(SelectedCompTourCatKumite);
                var competitorsKumite = await _tourCompetitorRepository
                    .GetCompetitorToursByCatKumiteIdAsync(SelectedTourCatKumite.TourCatKumiteId);
                CompTourCatKumite = new ObservableCollection<TourCompetitorDto>(competitorsKumite);
                SelectedCompTourCatKumite = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania zawodnika z kategorii kumite: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExecuteSetCompAutomaticCatKataCommand(object obj)
        {
            if (SelectedTourToSetCompToCat == null) return;
            try
            {
                var result = await _tourCompetitorRepository.SetCompToCatKataAutomatic(SelectedTourToSetCompToCat.TourId);
                MessageBox.Show(result, "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas automatycznego przypisywania zawodników do kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteSetCompAutomaticCatKumiteCommand(object obj)
        {
            if (SelectedTourToSetCompToCat == null) return;
            try
            {
                var result = await _tourCompetitorRepository.SetCompToKumiteCatAutomatic(SelectedTourToSetCompToCat.TourId);
                MessageBox.Show(result, "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas automatycznego przypisywania zawodników do kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteSetFightsCommand(object obj)
        {
            if (SelectedTourToSetCompToCat == null) return;
            try
            {
                var result = await _fightRepository.SetFightsAsync(SelectedTourToSetCompToCat.TourId);
                if(result) MessageBox.Show("Stworzono drabinki walk.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                else MessageBox.Show("Nie można stworzyć drabinek walk. Sprawdź czy są zawodnicy w kategoriach.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas ustalania walk: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
