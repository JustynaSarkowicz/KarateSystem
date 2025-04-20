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
        private bool _isEditingExistingClub;
        private bool _isEditingExistingMat;
        private bool _isEditingExistingDegree;
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
        public bool IsEditingExistingDegree
        {
            get => _isEditingExistingDegree;
            set
            {
                _isEditingExistingDegree = value;
                OnPropertyChanged(nameof(IsEditingExistingDegree));
                OnPropertyChanged(nameof(IsAddingNewDegree));
            }
        }
        public bool IsAddingNewDegree => !IsEditingExistingDegree;
        
        public bool IsEditingExistingClub
        {
            get => _isEditingExistingClub;
            set
            {
                _isEditingExistingClub = value;
                OnPropertyChanged(nameof(IsEditingExistingClub));
                OnPropertyChanged(nameof(IsAddingNewClub));
            }
        }
        public bool IsAddingNewClub => !IsEditingExistingClub;
        
        public bool IsEditingExistingMat
        {
            get => _isEditingExistingMat;
            set
            {
                _isEditingExistingMat = value;
                OnPropertyChanged(nameof(IsEditingExistingMat));
                OnPropertyChanged(nameof(IsAddingNewMat));
            }
        }
        public bool IsAddingNewMat => !IsEditingExistingMat;

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
            CancelClubCommand = new ViewModelCommand(ExecuteCancelClubCommand);

            EditMatCommand = new ViewModelCommand(ExecuteEditMatCommand, CanEditMat);
            UpdateMatCommand = new ViewModelCommand(ExecuteUpdateMatCommand, CanCancelMatEdit);
            AddMatCommand = new ViewModelCommand(ExecuteAddMatCommand);
            CancelMatCommand = new ViewModelCommand(ExecuteCancelMatCommand);

            EditDegreeCommand = new ViewModelCommand(ExecuteEditDegreeCommand, CanEditDegree);
            UpdateDegreeCommand = new ViewModelCommand(ExecuteUpdateDegreeCommand, CanCancelDegreeEdit);
            AddDegreeCommand = new ViewModelCommand(ExecuteAddDegreeCommand);
            CancelDegreeCommand = new ViewModelCommand(ExecuteCancelDegreeCommand);

            LoadAsync();
        }

        private async void LoadAsync()
        {
            _allClubs = await _clubRepository.GetAllClubsAsync();
            Clubs = new ObservableCollection<ClubDto>(_allClubs);

            var mats = await _matRepository.GetAllMatAsync();
            Mats = new ObservableCollection<MatDto>(mats);

            var degrees = await _degreeRepository.GetAllDegreeAsync();
            Degrees = new ObservableCollection<DegreeDto>(degrees);
        }


        #region Club Commands
        private void FilterClubs()
        {
            Clubs = string.IsNullOrWhiteSpace(SearchText)
                ? new ObservableCollection<ClubDto>(_allClubs)
                : new ObservableCollection<ClubDto>(
                    _searchService.SearchInCollection(_allClubs, SearchText, "ClubName", "ClubPlace"));
        }
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

            IsEditingExistingClub = true;
        }

        private async void ExecuteUpdateClubCommand(object obj)
        {
            try
            {
                if (!IsClubValid(EditingClub) || SelectedClub == null) return;

                SelectedClub.ClubId = EditingClub.ClubId;
                SelectedClub.ClubName = EditingClub.ClubName;
                SelectedClub.ClubPlace = EditingClub.ClubPlace;

                await _clubRepository.UpdateClubAsync(SelectedClub);

                Clubs = new ObservableCollection<ClubDto>(await _clubRepository.GetAllClubsAsync());
                ExecuteCancelClubCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji klubu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExecuteAddClubCommand(object obj)
        {
            try
            {
                if (!IsClubValid(EditingClub)) return;

                await _clubRepository.AddClubAsync(EditingClub);

                _allClubs = await _clubRepository.GetAllClubsAsync();
                Clubs = new ObservableCollection<ClubDto>(_allClubs);
                ExecuteCancelClubCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania klubu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelClubCommand(object obj)
        {
            EditingClub = new ClubDto();
            SelectedClub = null;
            IsEditingExistingClub = false;
        }

        private bool IsClubValid(ClubDto club)
        {
            if(string.IsNullOrWhiteSpace(club.ClubName) || string.IsNullOrWhiteSpace(club.ClubPlace))
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
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
            IsEditingExistingMat = true;
        }
        private async void ExecuteUpdateMatCommand(object obj)
        {
            try
            {
                if (!IsMatValid(EditingMat) || SelectedMat == null) return;

                SelectedMat.MatId = EditingMat.MatId;
                SelectedMat.MatName = EditingMat.MatName;

                await _matRepository.UpdateMatAsync(SelectedMat);

                Mats = new ObservableCollection<MatDto>(await _matRepository.GetAllMatAsync());
                ExecuteCancelMatCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji maty: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteAddMatCommand(object obj)
        {
            try
            {
                if (!IsMatValid(EditingMat)) return;

                await _matRepository.AddMatAsync(EditingMat);

                Mats = new ObservableCollection<MatDto>(await _matRepository.GetAllMatAsync());
                ExecuteCancelMatCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania maty: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteCancelMatCommand(object obj)
        {
            EditingMat = new MatDto();
            SelectedMat = null;
            IsEditingExistingMat = false;
        }
        private bool IsMatValid(MatDto mat)
        {
            if (string.IsNullOrWhiteSpace(mat.MatName))
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
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

            IsEditingExistingDegree = true;
        }

        private async void ExecuteUpdateDegreeCommand(object obj)
        {
            try
            {
                if (!IsDegreeValid(EditingDegree) || SelectedDegree == null) return;

                SelectedDegree.DegreeId = EditingDegree.DegreeId;
                SelectedDegree.DegreeName = EditingDegree.DegreeName;

                await _degreeRepository.UpdateDegreeAsync(SelectedDegree);
                
                Degrees = new ObservableCollection<DegreeDto>(await _degreeRepository.GetAllDegreeAsync());
                ExecuteCancelDegreeCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji stopnia: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExecuteAddDegreeCommand(object obj)
        {
            try
            {
                if (!IsDegreeValid(EditingDegree)) return;

                await _degreeRepository.AddDegreeAsync(EditingDegree);

                Degrees = new ObservableCollection<DegreeDto>(await _degreeRepository.GetAllDegreeAsync());
                ExecuteCancelDegreeCommand(obj);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania stopnia: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelDegreeCommand(object obj)
        {
            EditingDegree = new DegreeDto();
            SelectedDegree = null;
            IsEditingExistingDegree = false;
        }
        private bool IsDegreeValid(DegreeDto degree)
        {
            if (string.IsNullOrWhiteSpace(degree.DegreeName))
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        #endregion
    }
}
