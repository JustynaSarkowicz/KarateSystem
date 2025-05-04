using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository;
using KarateSystem.Repository.Interfaces;
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
    public class KataKumiteViewModel : ViewModelBase
    {
        #region Fields
        private TournamentDto _editingTour;
        private TournamentDto _selectedTourToChoseCat;
        private ObservableCollection<TournamentDto> _tournaments;
        private TourCatKataDto _selectedTourCatKata;
        private TourCatKumiteDto _selectedTourCatKumite;
        private ObservableCollection<TourCatKataDto> _tourCatKatas;
        private ObservableCollection<TourCatKumiteDto> _tourCatKumites;
        private KataDto _selectedKata;
        private KataDto _editingKata;
        private ObservableCollection<KataDto> _katas;
        private FightDto _selectedFight;
        private FightDto _editingFight;
        private ObservableCollection<FightDto> _fights;
        private OvertimePlaceOption _selectedOvertime;
        private WalkoverOption _selectedWalkover;
        private TourCompetitorDto _selectedCompWinner;
        private ObservableCollection<TourCompetitorDto> _winnerList;
        private bool _isEnabled;
        private ObservableCollection<int> _rounds;
        private int _selectedRound;

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITourCompetitorRepository _tourCompetitorRepository;
        private readonly ITourCatKumiteRepository _tourCatKumiteRepository;
        private readonly ITourCatKataRepository _tourCatKataRepository;
        private readonly IKataRepository _kataRepository;
        private readonly IFightRepository _fightsRepository;
        #endregion

        #region Properties
        public ObservableCollection<int> Rounds
        {
            get => _rounds;
            set
            {
                _rounds = value;
                OnPropertyChanged(nameof(Rounds));
            }
        }
        public int SelectedRound
        {
            get => _selectedRound;
            set
            {
                _selectedRound = value;
                OnPropertyChanged(nameof(SelectedRound));
                LoadFightsForSelectedRound();
            }
        }
        public ObservableCollection<TourCompetitorDto> WinnerList
        {
            get => _winnerList;
            set
            {
                _winnerList = value;
                OnPropertyChanged(nameof(WinnerList));
            }
        }
        public TourCompetitorDto SelectedCompWinner
        {
            get => _selectedCompWinner;
            set
            {
                _selectedCompWinner = value;
                OnPropertyChanged(nameof(SelectedCompWinner));
            }
        }
        public WalkoverOption SelectedWalkover
        {
            get => _selectedWalkover;
            set
            {
                _selectedWalkover = value;
                OnPropertyChanged(nameof(SelectedWalkover));
            }
        }
        public FightDto EditingFight
        {
            get => _editingFight;
            set
            {
                _editingFight = value;
                OnPropertyChanged(nameof(EditingFight));
            }
        }
        public FightDto SelectedFight
        {
            get => _selectedFight;
            set
            {
                _selectedFight = value;
                OnPropertyChanged(nameof(SelectedFight));
            }
        }
        public ObservableCollection<FightDto> Fights
        {
            get => _fights;
            set
            {
                _fights = value;
                OnPropertyChanged(nameof(Fights));
            }
        }
        public OvertimePlaceOption SelectedOvertime
        {
            get => _selectedOvertime;
            set
            {
                _selectedOvertime = value;
                OnPropertyChanged(nameof(SelectedOvertime));
            }
        }
        public bool IsEnabled 
        {
           get =>  _isEnabled;
           set
           {
               _isEnabled = value;
               OnPropertyChanged(nameof(IsEnabled));
           }
        }
        public KataDto EditingKata
        {
            get => _editingKata;
            set
            {
                _editingKata = value;
                OnPropertyChanged(nameof(EditingKata));
            }
        }
        public KataDto SelectedKata
        {
            get => _selectedKata;
            set
            {
                _selectedKata = value;
                OnPropertyChanged(nameof(SelectedKata));
            }
        }
        public ObservableCollection<KataDto> Katas
        {
            get => _katas;
            set
            {
                _katas = value;
                OnPropertyChanged(nameof(Katas));
            }
        }
        public TournamentDto EditingTour
        {
            get => _editingTour;
            set
            {
                _editingTour = value;
                OnPropertyChanged(nameof(EditingTour));                
            }
        }
        public TourCatKumiteDto SelectedTourCatKumite
        {
            get => _selectedTourCatKumite;
            set
            {
                _selectedTourCatKumite = value;
                OnPropertyChanged(nameof(SelectedTourCatKumite));
                LoadFightsForKumiteCategoryAsync();
            }
        }
        public TourCatKataDto SelectedTourCatKata
        {
            get => _selectedTourCatKata;
            set
            {
                _selectedTourCatKata = value;
                OnPropertyChanged(nameof(SelectedTourCatKata));
                LoadKatasForKataCategoryAsync();
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
        public ObservableCollection<TournamentDto> Tournaments
        {
            get => _tournaments;
            set
            {
                _tournaments = value;
                OnPropertyChanged(nameof(Tournaments));
            }
        }
        public TournamentDto SelectedTourToChoseCat
        {
            get => _selectedTourToChoseCat;
            set
            {
                _selectedTourToChoseCat = value;
                OnPropertyChanged(nameof(SelectedTourToChoseCat));
            }
        }
        #endregion

        #region Commands
        public ICommand ChoseTourToSetCatCommand { get; }
        public ICommand SetGradesToKataCommand { get; }
        public ICommand EditKataCommand { get; }
        public ICommand SortKataCommand { get; }
        public ICommand EditFightCommand { get; }
        public ICommand SetFightCommand { get; }
        public ICommand GenerateNextRoundCommand { get; }
        #endregion

        public KataKumiteViewModel(ITournamentRepository tournamentRepository,
            ITourCatKataRepository tourCatKataRepository,
            ITourCatKumiteRepository tourCatKumiteRepository,
            IKataRepository kataRepository,
            IFightRepository fightRepository,
            ITourCompetitorRepository tourCompetitorRepository)
        {
            _tournamentRepository = tournamentRepository;
            _tourCatKataRepository = tourCatKataRepository;
            _tourCatKumiteRepository = tourCatKumiteRepository;
            _kataRepository = kataRepository;
            _fightsRepository = fightRepository;
            _tourCompetitorRepository = tourCompetitorRepository;

            ChoseTourToSetCatCommand = new ViewModelCommand(ExecuteFillCatInTourCommand);
            EditKataCommand = new ViewModelCommand(ExecuteEditKataCommand, CanEditKata);
            SetGradesToKataCommand = new ViewModelCommand(ExecuteSetGradesToKataCommand, CanEditKata);
            SortKataCommand = new ViewModelCommand(ExecuteSortKataCommand);

            EditFightCommand = new ViewModelCommand(ExecuteEditFightCommand, CanEditFight);
            SetFightCommand = new ViewModelCommand(ExecuteSetFightCommand, CanEditFight);
            GenerateNextRoundCommand = new ViewModelCommand(ExecuteGenerateNextRound);

            _ = LoadAsync();
        }
        public async Task LoadAsync()
        {
            var tour = await _tournamentRepository.GetOnlyActiveTournamentsAsync().ConfigureAwait(false);
            Tournaments = new ObservableCollection<TournamentDto>(tour);
            EditingTour = new TournamentDto
            {
                TourCatKatas = new List<TourCatKataDto>(),
                TourCatKumites = new List<TourCatKumiteDto>()
            };

            EditingKata = new KataDto
            {
                Numeration = null
            };
            SelectedOvertime = OvertimeOptions.FirstOrDefault(x => x.Value == EditingKata.Overtime);

            EditingFight = new FightDto
            {
                FightWalkover = false
            };
            SelectedWalkover = WalkoverOptions.FirstOrDefault(x => x.Value == EditingFight.FightWalkover);
        }

        private async void ExecuteFillCatInTourCommand(object obj)
        {
            if (SelectedTourToChoseCat == null) return;
            if (SelectedTourToChoseCat.Status == 3) IsEnabled = false;
            else IsEnabled = true;
            TourCatKumites = new ObservableCollection<TourCatKumiteDto>(
                await _tourCatKumiteRepository.GetCatKumiteByIdTourAsync(SelectedTourToChoseCat.TourId));
            TourCatKatas = new ObservableCollection<TourCatKataDto>(
                await _tourCatKataRepository.GetCatKataByIdTourAsync(SelectedTourToChoseCat.TourId));
        }

        private bool CanEditKata(object obj) => SelectedKata != null;
        private bool CanEditFight(object obj) => SelectedFight != null;
        private async void LoadKatasForKataCategoryAsync()
        {
            if (SelectedTourCatKata == null)
            {
                Katas = new ObservableCollection<KataDto>();
                return;
            }

            var katas = await _kataRepository.GetKatasByTourCatKataIdAsync(SelectedTourCatKata.TourCatKataId);
            Katas = new ObservableCollection<KataDto>(katas);
        }
        private async void LoadFightsForKumiteCategoryAsync()
        {
            if (SelectedTourCatKumite == null)
            {
                Fights = new ObservableCollection<FightDto>();
                return;
            }

            var fights = await _fightsRepository.GetFightsByTourAsync(SelectedTourCatKumite.TourCatKumiteId);
            Fights = new ObservableCollection<FightDto>(fights);

            _ = LoadRoundsAsync();
        }

        private void ExecuteEditKataCommand(object obj)
        {
            if (SelectedKata == null) return;

            EditingKata = new KataDto
            {
                KataId = SelectedKata.KataId,
                Numeration = SelectedKata.Numeration,
                KataRate1 = SelectedKata.KataRate1,
                KataRate2 = SelectedKata.KataRate2,
                KataRate3 = SelectedKata.KataRate3,
                KataRate4 = SelectedKata.KataRate4,
                KataRate5 = SelectedKata.KataRate5,
                CompFirstName = SelectedKata.CompFirstName,
                CompLastName = SelectedKata.CompLastName,
                Overtime = SelectedKata.Overtime
            };

            SelectedOvertime = OvertimeOptions.FirstOrDefault(x => x.Value == EditingKata.Overtime);
        }
        private async void ExecuteEditFightCommand(object obj)
        {
            if (SelectedFight == null) return;

            EditingFight = new FightDto
            {
                FightId = SelectedFight.FightId,
                FightNumber = SelectedFight.FightNumber,
                TourCatKumiteId = SelectedFight.TourCatKumiteId,
                RedCompetitorId = SelectedFight.RedCompetitorId,
                RedCompetitorName = SelectedFight.RedCompetitorName,
                BlueCompetitorId = SelectedFight.BlueCompetitorId,
                BlueCompetitorName = SelectedFight.BlueCompetitorName,
                WinnerId = SelectedFight.WinnerId,
                WinnerName = SelectedFight.WinnerName,
                RedCompetitorScore = SelectedFight.RedCompetitorScore,
                BlueCompetitorScore = SelectedFight.BlueCompetitorScore,
                FightTime = SelectedFight.FightTime,
                FightNumOverTime = SelectedFight.FightNumOverTime,
                FightWalkover = SelectedFight.FightWalkover,
            };
            WinnerList = new ObservableCollection<TourCompetitorDto>();

            if (SelectedFight.RedCompetitorId != null)
            {
                var redCompetitor = await _tourCompetitorRepository.GetTourCompetitorByIdAsync(Convert.ToInt32(SelectedFight.RedCompetitorId)); 
                if (redCompetitor != null)
                {
                    WinnerList.Add(new TourCompetitorDto
                    {
                        TourCompId = redCompetitor.TourCompId,
                        CompFullName = redCompetitor.CompFirstName + " " + redCompetitor.CompLastName
                    });
                }
            }

            if (SelectedFight.BlueCompetitorId != null)
            {
                var blueCompetitor = await _tourCompetitorRepository.GetTourCompetitorByIdAsync(Convert.ToInt32(SelectedFight.BlueCompetitorId));
                if (blueCompetitor != null)
                {
                    WinnerList.Add(new TourCompetitorDto
                    {
                        TourCompId = blueCompetitor.TourCompId,
                        CompFullName = blueCompetitor.CompFirstName + " " + blueCompetitor.CompLastName
                    });
                }
            }
            if (SelectedFight.WinnerId == null) SelectedCompWinner = null;
            else SelectedCompWinner = WinnerList.FirstOrDefault(x => x.TourCompId == SelectedFight.WinnerId);

            SelectedWalkover = WalkoverOptions.FirstOrDefault(x => x.Value == EditingFight.FightWalkover);
        }
        private async void ExecuteSetFightCommand(object obj)
        {
            if (SelectedFight == null) return;
            if (EditingFight.RedCompetitorScore < 0 || EditingFight.RedCompetitorScore == null ||
                EditingFight.BlueCompetitorScore < 0 || EditingFight.BlueCompetitorScore == null ||
                SelectedCompWinner == null)
            {
                MessageBox.Show("Wszystkie oceny muszą być uzupełnione!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                SelectedFight.RedCompetitorScore = EditingFight.RedCompetitorScore;
                SelectedFight.BlueCompetitorScore = EditingFight.BlueCompetitorScore;
                SelectedFight.FightWalkover = SelectedWalkover.Value;
                SelectedFight.WinnerId = SelectedCompWinner?.TourCompId;
                SelectedFight.FightNumOverTime = EditingFight.FightNumOverTime;
                SelectedFight.FightTime = EditingFight.FightTime;

                await _fightsRepository.UpdateFightsAsync(SelectedFight);
                LoadFightsForSelectedRound();
                SelectedFight = null;
                EditingFight = new FightDto
                {
                    FightWalkover = false
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawanie ocen do kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteSetGradesToKataCommand(object obj)
        {
            if (SelectedKata == null) return;
            if (EditingKata.KataRate1 < 0 || EditingKata.KataRate1 == null ||
               EditingKata.KataRate2 < 0 || EditingKata.KataRate2 == null ||
               EditingKata.KataRate3 < 0 || EditingKata.KataRate3 == null ||
               EditingKata.KataRate4 < 0 || EditingKata.KataRate4 == null ||
               EditingKata.KataRate5 < 0 || EditingKata.KataRate5 == null)
            {
                MessageBox.Show("Wszystkie oceny muszą być uzupełnione!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                SelectedKata.KataRate1 = EditingKata.KataRate1;
                SelectedKata.KataRate2 = EditingKata.KataRate2;
                SelectedKata.KataRate3 = EditingKata.KataRate3;
                SelectedKata.KataRate4 = EditingKata.KataRate4;
                SelectedKata.KataRate5 = EditingKata.KataRate5;
                SelectedKata.Overtime = SelectedOvertime.Value;

                await _kataRepository.UpdateGradesOnKataAsync(SelectedKata);

                var katas = await _kataRepository.GetKatasByTourCatKataIdAsync(SelectedTourCatKata.TourCatKataId);
                Katas = new ObservableCollection<KataDto>(katas);
                SelectedKata = null; 
                EditingKata = new KataDto
                {
                    Numeration = null
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawanie ocen do kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteSortKataCommand(object obj)
        {
            if (SelectedTourCatKata == null) return;
            Katas = new ObservableCollection<KataDto>(Katas.OrderByDescending(x => x.KataScore));
        }

        #region Rounds
        private async Task LoadRoundsAsync()
        {
            if(SelectedTourCatKumite == null)
            {
                Rounds = new ObservableCollection<int>();
                return;
            }
            var allFights = await _fightsRepository.GetFightsByTourAsync(SelectedTourCatKumite.TourCatKumiteId);
            var roundNumbers = allFights
                .Select(f => f.Round)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            Rounds = new ObservableCollection<int>(roundNumbers);

            if (!Rounds.Contains(SelectedRound))
                SelectedRound = Rounds.FirstOrDefault();
        }
        private async void LoadFightsForSelectedRound()
        {
            var fights = await _fightsRepository.GetFightsByTourAndRoundAsync(SelectedTourCatKumite.TourCatKumiteId, SelectedRound);
            Fights = new ObservableCollection<FightDto>(fights);
        }

        private async void ExecuteGenerateNextRound(object obj)
        {
            int currentRound = SelectedRound;
            int nextRound = currentRound + 1;

            var fightsInCurrentRound = await _fightsRepository.GetFightsByTourAndRoundAsync(SelectedTourCatKumite.TourCatKumiteId, currentRound);

            var incompleteFights = fightsInCurrentRound.Where(f => !f.WinnerId.HasValue).ToList();

            if (incompleteFights.Any())
            {
                var result = MessageBox.Show(
                    "Nie wszystkie walki mają wybranego zwycięzcę.\n" +
                    "Zawodnicy z tych walk zostaną pominięci w dalszej rundzie.\n\n" +
                    "Czy chcesz wrócić i uzupełnić brakujące walki?",
                    "Brak zwycięzców",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    return; 
            }

            var winners = fightsInCurrentRound
                .Where(f => f.WinnerId.HasValue)
                .Select(f => f.WinnerId.Value)
                .ToList();

            if (winners.Count < 2)
            {
                MessageBox.Show("Za mało zwycięzców do utworzenia nowej rundy.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newFights = new List<FightDto>();
            for (int i = 0; i < winners.Count; i += 2)
            {
                var redId = winners[i];
                int? blueId = (i + 1 < winners.Count) ? winners[i + 1] : null;

                newFights.Add(new FightDto
                {
                    TourCatKumiteId = SelectedTourCatKumite.TourCatKumiteId,
                    RedCompetitorId = redId,
                    BlueCompetitorId = blueId,
                    Round = nextRound
                });
            }

            await _fightsRepository.AddFightsAsync(newFights);

            MessageBox.Show($"Utworzono {newFights.Count} walk dla rundy {nextRound}.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            await LoadRoundsAsync();
            SelectedRound = nextRound;
        }

        #endregion
    }
}
