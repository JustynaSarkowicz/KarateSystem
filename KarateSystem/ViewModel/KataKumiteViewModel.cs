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
        private OvertimePlaceOption _selectedOvertime;
        private bool _isEnabled;

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITourCatKumiteRepository _tourCatKumiteRepository;
        private readonly ITourCatKataRepository _tourCatKataRepository;
        private readonly IKataRepository _kataRepository;
        #endregion

        #region Properties
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
        #endregion

        public KataKumiteViewModel(ITournamentRepository tournamentRepository,
            ITourCatKataRepository tourCatKataRepository,
            ITourCatKumiteRepository tourCatKumiteRepository,
            IKataRepository kataRepository)
        {
            _tournamentRepository = tournamentRepository;
            _tourCatKataRepository = tourCatKataRepository;
            _tourCatKumiteRepository = tourCatKumiteRepository;
            _kataRepository = kataRepository;

            ChoseTourToSetCatCommand = new ViewModelCommand(ExecuteFillCatInTourCommand);
            EditKataCommand = new ViewModelCommand(ExecuteEditKataCommand, CanEditKata);
            SetGradesToKataCommand = new ViewModelCommand(ExecuteSetGradesToKataCommand, CanEditKata);
            SortKataCommand = new ViewModelCommand(ExecuteSortKataCommand);

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

        #region Kata
        private bool CanEditKata(object obj) => SelectedKata != null;
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
        #endregion
    }
}
