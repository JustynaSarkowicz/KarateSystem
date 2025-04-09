using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using System.Windows;

namespace KarateSystem.ViewModel
{
    public class ClubsDegreesMatsViewModel : ViewModelBase
    {
        #region Fields
        private ClubDto _selectedClub;
        private ClubDto _editingClub;
        private MatDto _selectedMat;
        private MatDto _editingMat;
        private DegreeDto _selectedDegree;
        private DegreeDto _editingDegree;
        private string _searchText;

        private ObservableCollection<ClubDto> _clubs;
        private ObservableCollection<MatDto> _mats;
        private ObservableCollection<DegreeDto> _degrees;
        private List<ClubDto> _allClubs;

        private readonly IClubRepository _clubRepository;
        private readonly ISearchService _searchService;
        private readonly IMatRepository _matRepository;
        private readonly IDegreeRepository _degreeRepository;
        private readonly IMapper _mapper;
        #endregion
        #region ICommands
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
        #endregion
        #region Properties
        public ObservableCollection<ClubDto> Clubs
        {
            get => _clubs;
            set { _clubs = value; OnPropertyChanged(nameof(Clubs)); }
        }

        public ObservableCollection<MatDto> Mats
        {
            get => _mats;
            set { _mats = value; OnPropertyChanged(nameof(Mats)); }
        }

        public ObservableCollection<DegreeDto> Degrees
        {
            get => _degrees;
            set { _degrees = value; OnPropertyChanged(nameof(Degrees)); }
        }

        public ClubDto SelectedClub
        {
            get => _selectedClub;
            set { _selectedClub = value; OnPropertyChanged(nameof(SelectedClub)); }
        }

        public ClubDto EditingClub
        {
            get => _editingClub;
            set { _editingClub = value; OnPropertyChanged(nameof(EditingClub)); }
        }

        public MatDto SelectedMat
        {
            get => _selectedMat;
            set { _selectedMat = value; OnPropertyChanged(nameof(SelectedMat)); }
        }

        public MatDto EditingMat
        {
            get => _editingMat;
            set { _editingMat = value; OnPropertyChanged(nameof(EditingMat)); }
        }

        public DegreeDto SelectedDegree
        {
            get => _selectedDegree;
            set { _selectedDegree = value; OnPropertyChanged(nameof(SelectedDegree)); }
        }

        public DegreeDto EditingDegree
        {
            get => _editingDegree;
            set { _editingDegree = value; OnPropertyChanged(nameof(EditingDegree)); }
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
        #endregion
        public ClubsDegreesMatsViewModel(
            IClubRepository clubRepository,
            ISearchService searchService,
            IMatRepository matRepository,
            IDegreeRepository degreeRepository,
            IMapper mapper)
        {
            _clubRepository = clubRepository;
            _searchService = searchService;
            _matRepository = matRepository;
            _degreeRepository = degreeRepository;
            _mapper = mapper;

            Clubs = new ObservableCollection<ClubDto>();
            Mats = new ObservableCollection<MatDto>();
            Degrees = new ObservableCollection<DegreeDto>();

            EditingClub = new ClubDto();
            EditingMat = new MatDto();
            EditingDegree = new DegreeDto();

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

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            _allClubs = await _clubRepository.GetAllClubsAsync();
            Clubs = new ObservableCollection<ClubDto>(_allClubs);

            var mats = await _matRepository.GetAllMatAsync();
            Mats = new ObservableCollection<MatDto>(mats);

            var degrees = await _degreeRepository.GetAllDegreeAsync();
            Degrees = new ObservableCollection<DegreeDto>(degrees);
        }

        private void FilterClubs()
        {
            Clubs = string.IsNullOrWhiteSpace(SearchText)
                ? new ObservableCollection<ClubDto>(_allClubs)
                : new ObservableCollection<ClubDto>(
                    _searchService.SearchInCollection(_allClubs, SearchText, "ClubName", "ClubPlace"));
        }

        #region Club Commands

        private bool CanEditClub(object obj) => SelectedClub != null;
        private bool CanCancelClubEdit(object obj) => EditingClub != null && SelectedClub != null;

        private void ExecuteEditClubCommand(object obj)
        {
            EditingClub = new ClubDto
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
                if (!Clubs.Contains(SelectedClub)) return;

                SelectedClub.ClubId = EditingClub.ClubId;
                SelectedClub.ClubName = EditingClub.ClubName;
                SelectedClub.ClubPlace = EditingClub.ClubPlace;
                var result = await _clubRepository.UpdateClubAsync(SelectedClub);
                if(!result)
                {
                    MessageBox.Show("Dodaj nazwę i miejscę klubu.",
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Clubs = new ObservableCollection<ClubDto>(_allClubs);
                EditingClub = new ClubDto();
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

            var result = await _clubRepository.AddClubAsync(EditingClub);
            if(!result)
            {
                MessageBox.Show("Klub o takiej nazwie już istnieje.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Clubs.Add(EditingClub);
            EditingClub = new ClubDto();
        }

        private void ExecuteCancelClubCommand(object obj)
        {
            EditingClub = new ClubDto();
            SelectedClub = null;
        }

        #endregion

        #region Mat Commands

        private bool CanEditMat(object obj) => SelectedMat != null;
        private bool CanCancelMatEdit(object obj) => EditingMat != null && SelectedMat != null;

        private void ExecuteEditMatCommand(object obj)
        {
            EditingMat = new MatDto
            {
                MatId = SelectedMat.MatId,
                MatName = SelectedMat.MatName
            };
        }

        private async void ExecuteUpdateMatCommand(object obj)
        {
            try
            {
                if (!Mats.Contains(SelectedMat)) return;
                SelectedMat.MatId = EditingMat.MatId;
                SelectedMat.MatName = EditingMat.MatName;
                var result = await _matRepository.UpdateMatAsync(SelectedMat);
                if (!result)
                {
                    MessageBox.Show("Dodaj nazwę maty.",
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Mats = new ObservableCollection<MatDto>(_mats);
                EditingMat = new MatDto();
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

            var result = await _matRepository.AddMatAsync(EditingMat);
            if (!result)
            {
                MessageBox.Show("Mata o takiej nazwie już istnieje.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Mats.Add(EditingMat);
            EditingMat = new MatDto();
        }

        private void ExecuteCancelMatCommand(object obj)
        {
            EditingMat = new MatDto();
            SelectedMat = null;
        }

        #endregion

        #region Degree Commands

        private bool CanEditDegree(object obj) => SelectedDegree != null;
        private bool CanCancelDegreeEdit(object obj) => EditingDegree != null && SelectedDegree != null;

        private void ExecuteEditDegreeCommand(object obj)
        {
            EditingDegree = new DegreeDto
            {
                DegreeId = SelectedDegree.DegreeId,
                DegreeName = SelectedDegree.DegreeName
            };
        }

        private async void ExecuteUpdateDegreeCommand(object obj)
        {
            try
            {
                if (!Degrees.Contains(SelectedDegree)) return;
                SelectedDegree.DegreeId = EditingDegree.DegreeId;
                SelectedDegree.DegreeName = EditingDegree.DegreeName;
                var result = await _degreeRepository.UpdateDegreeAsync(SelectedDegree);
                if (!result)
                {
                    MessageBox.Show("Dodaj nazwę stopnia.",
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Degrees = new ObservableCollection<DegreeDto>(_degrees);
                EditingDegree = new DegreeDto();
                SelectedDegree = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating degree: {ex.Message}");
            }
        }

        private async void ExecuteAddDegreeCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(EditingDegree?.DegreeName))
                return;

            var result = await _degreeRepository.AddDegreeAsync(EditingDegree);
            if (!result)
            {
                MessageBox.Show("Stopień o takiej nazwie już istnieje.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Degrees.Add(EditingDegree);
            EditingDegree = new DegreeDto();
        }

        private void ExecuteCancelDegreeCommand(object obj)
        {
            EditingDegree = new DegreeDto();
            SelectedDegree = null;
        }

        #endregion
    }
}
