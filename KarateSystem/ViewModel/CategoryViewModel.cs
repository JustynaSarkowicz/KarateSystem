using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace KarateSystem.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        #region Fields
        private string _searchTextCatKata;
        private string _searchTextCatKumite;
        private bool _isEditingExistingCatKata;
        private bool _isEditingExistingCatKumite;
        private KataCategoryDto _selectedKataCategory;
        private KataCategoryDto _editingKataCategory;
        private DegreeDto _selectedDegreeToAdd;
        private CatKataDegreeDto _selectedCatKataDegree;
        private GenderOption2 _selectedGenderCatKata;
        private GenderOption _selectedGenderCatKumite;
        private KumiteCategoryDto _selectedKumiteCategory;
        private KumiteCategoryDto _editingKumiteCategory;

        private ObservableCollection<KataCategoryDto> _kataCategories;
        private ObservableCollection<DegreeDto> _degrees;
        private List<KataCategoryDto> _allKataCategories;
        private ObservableCollection<KumiteCategoryDto> _kumiteCategories;
        private List<KumiteCategoryDto> _allKumiteCategories;

        private readonly IKataCategoryRepository _kataCategoryRepository;
        private readonly IKumiteCategoryRepository _kumiteCategoryRepository;
        private readonly IDegreeRepository _degreeRepository;
        private readonly ICatKataDegreeRepository _catKataDegreeRepository;
        private readonly ISearchService _searchService;
        #endregion

        #region Commands
        public ICommand EditKataCategoryCommand { get; }
        public ICommand CancelKataCategoryCommand { get; }
        public ICommand UpdateKataCategoryCommand { get; }
        public ICommand AddKataCategoryCommand { get; }
        public ICommand EditKumiteCategoryCommand { get; }
        public ICommand CancelKumiteCategoryCommand { get; }
        public ICommand UpdateKumiteCategoryCommand { get; }
        public ICommand AddKumiteCategoryCommand { get; }
        public ICommand AddDegreeCommand { get; }
        public ICommand RemoveDegreeCommand { get; }
        #endregion

        #region Properties
        public bool IsEditingExistingCatKata
        {
            get => _isEditingExistingCatKata;
            set
            {
                _isEditingExistingCatKata = value;
                OnPropertyChanged(nameof(IsEditingExistingCatKata));
                OnPropertyChanged(nameof(IsAddingNewCatKata));
            }
        }
        public bool IsAddingNewCatKata => !IsEditingExistingCatKata;
        public bool IsEditingExistingCatKumite
        {
            get => _isEditingExistingCatKumite;
            set
            {
                _isEditingExistingCatKumite = value;
                OnPropertyChanged(nameof(IsEditingExistingCatKumite));
                OnPropertyChanged(nameof(IsAddingNewCatKumite));
            }
        }
        public bool IsAddingNewCatKumite => !IsEditingExistingCatKumite;
        public ObservableCollection<KumiteCategoryDto> KumiteCategories
        {
            get => _kumiteCategories;
            set
            {
                _kumiteCategories = value;
                OnPropertyChanged(nameof(KumiteCategories));
            }
        }
        public ObservableCollection<KataCategoryDto> KataCategories
        {
            get => _kataCategories;
            set
            {
                _kataCategories = value;
                OnPropertyChanged(nameof(KataCategories));
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
        public KumiteCategoryDto EditingKumiteCategory
        {
            get => _editingKumiteCategory;
            set
            {
                _editingKumiteCategory = value;
                OnPropertyChanged(nameof(EditingKumiteCategory));
            }
        }
        public DegreeDto SelectedDegreeToAdd
        {
            get => _selectedDegreeToAdd;
            set
            {
                _selectedDegreeToAdd = value;
                OnPropertyChanged(nameof(SelectedDegreeToAdd));
            }
        }
        public CatKataDegreeDto SelectedCatKataDegree
        {
            get => _selectedCatKataDegree;
            set
            {
                _selectedCatKataDegree = value;
                OnPropertyChanged(nameof(SelectedCatKataDegree));
            }
        }
        public KataCategoryDto EditingKataCategory
        {
            get => _editingKataCategory;
            set
            {
                _editingKataCategory = value;
                OnPropertyChanged(nameof(EditingKataCategory));
                OnPropertyChanged(nameof(EditingKataCategory.CatKataDegrees));
            }
        }
        public GenderOption2 SelectedGenderCatKata
        {
            get => _selectedGenderCatKata;
            set
            {
                _selectedGenderCatKata = value;
                OnPropertyChanged(nameof(SelectedGenderCatKata));
            }
        }
        public GenderOption SelectedGenderCatKumite
        {
            get => _selectedGenderCatKumite;
            set
            {
                _selectedGenderCatKumite = value;
                OnPropertyChanged(nameof(SelectedGenderCatKumite));
            }
        }
        public string SearchTextCatKata
        {
            get => _searchTextCatKata;
            set
            {
                _searchTextCatKata = value;
                OnPropertyChanged(nameof(SearchTextCatKata));
                FilterCatKata();
            }
        }
        public string SearchTextCatKumite
        {
            get => _searchTextCatKumite;
            set
            {
                _searchTextCatKumite = value;
                OnPropertyChanged(nameof(SearchTextCatKumite));
                FilterCatKumite();
            }
        }
        #endregion
        public CategoryViewModel(IKataCategoryRepository kataCategoryRepository,
            IDegreeRepository degreeRepository,
            ICatKataDegreeRepository catKataDegreeRepository,
            ISearchService searchService,
            IKumiteCategoryRepository kumiteCategoryRepository)
        {
            _kataCategoryRepository = kataCategoryRepository;
            _degreeRepository = degreeRepository;
            _catKataDegreeRepository = catKataDegreeRepository;
            _searchService = searchService;
            _kumiteCategoryRepository = kumiteCategoryRepository;

            EditKataCategoryCommand = new ViewModelCommand(ExecuteEditKataCategoryCommand, CanEditKataCategory);
            CancelKataCategoryCommand = new ViewModelCommand(ExecuteCancelKataCategoryCommand);
            UpdateKataCategoryCommand = new ViewModelCommand(ExecuteUpdateKataCategoryCommand, CanCancelKataCategory);
            AddKataCategoryCommand = new ViewModelCommand(ExecuteAddKataCategoryCommand);

            AddDegreeCommand = new ViewModelCommand(ExecuteAddDegreeCommand, CanAddDegree);
            RemoveDegreeCommand = new ViewModelCommand(ExecuteRemoveDegreeCommand, CanRemoveDegree);

            EditKumiteCategoryCommand = new ViewModelCommand(ExecuteEditKumiteCategoryCommand, CanEditKumiteCategory);
            CancelKumiteCategoryCommand = new ViewModelCommand(ExecuteCancelKumiteCategoryCommand);
            UpdateKumiteCategoryCommand = new ViewModelCommand(ExecuteUpdateKumiteCategoryCommand, CanCancelKumiteCategory);
            AddKumiteCategoryCommand = new ViewModelCommand(ExecuteAddKumiteCategoryCommand);

            LoadAsync();
        }
        private async void LoadAsync()
        {
            _allKataCategories = await _kataCategoryRepository.GetAllKataCategoryAsync();
            KataCategories = new ObservableCollection<KataCategoryDto>(_allKataCategories);

            _allKumiteCategories = await _kumiteCategoryRepository.GetAllKumiteCategoryAsync();
            KumiteCategories = new ObservableCollection<KumiteCategoryDto>(_allKumiteCategories);

            var degrees = await _degreeRepository.GetAllDegreeAsync();
            Degrees = new ObservableCollection<DegreeDto>(degrees);

            EditingKumiteCategory = new KumiteCategoryDto();
            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
        }
       
        #region KataCategory
        private bool CanEditKataCategory(object obj) => SelectedKataCategory != null;
        private bool CanCancelKataCategory(object obj) => EditingKataCategory != null && SelectedKataCategory != null;

        private void FilterCatKata()
        {
            KataCategories = string.IsNullOrWhiteSpace(SearchTextCatKata)
                ? new ObservableCollection<KataCategoryDto>(_allKataCategories)
                : new ObservableCollection<KataCategoryDto>(
                    _searchService.SearchInCollection(_allKataCategories, SearchTextCatKata, "KataCatName"));
        }
        private void ExecuteEditKataCategoryCommand(object obj)
        {
            if(SelectedKataCategory == null) return;

            EditingKataCategory = new KataCategoryDto
            {
                KataCatId = SelectedKataCategory.KataCatId,
                KataCatName = SelectedKataCategory.KataCatName,
                KataCatGender = SelectedKataCategory.KataCatGender,
                KataCatAgeMin = SelectedKataCategory.KataCatAgeMin,
                KataCatAgeMax = SelectedKataCategory.KataCatAgeMax,
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>(
                SelectedKataCategory.CatKataDegrees ?? new List<CatKataDegreeDto>())
            };
            SelectedGenderCatKata = GenderOptions2?.FirstOrDefault(g => g.Value == EditingKataCategory.KataCatGender);
            IsEditingExistingCatKata = true;
        }
        private void ExecuteCancelKataCategoryCommand(object obj)
        {
            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
            SelectedGenderCatKata = null;
            IsEditingExistingCatKata = false;
            SelectedKataCategory = null;
        }
        private async void ExecuteAddKataCategoryCommand(object obj)
        {
            try
            {
                if (!IsKataCatValid(EditingKataCategory)) return;

                EditingKataCategory.KataCatGender = SelectedGenderCatKata.Value;

                await _kataCategoryRepository.AddKataCategoryAsync(EditingKataCategory);

                _allKataCategories = await _kataCategoryRepository.GetAllKataCategoryAsync();
                KataCategories = new ObservableCollection<KataCategoryDto>(_allKataCategories);
                ExecuteCancelKataCategoryCommand(obj);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteUpdateKataCategoryCommand(object obj)
        {
            try
            {
                if (!IsKataCatValid(EditingKataCategory) || SelectedKataCategory == null) return;

                SelectedKataCategory.KataCatName = EditingKataCategory.KataCatName;
                SelectedKataCategory.KataCatGender = SelectedGenderCatKata.Value;
                SelectedKataCategory.KataCatAgeMin = EditingKataCategory.KataCatAgeMin;
                SelectedKataCategory.KataCatAgeMax = EditingKataCategory.KataCatAgeMax;
                SelectedKataCategory.CatKataDegrees = EditingKataCategory.CatKataDegrees;

                await _kataCategoryRepository.UpdateKataCategoryAsync(SelectedKataCategory);

                KataCategories = new ObservableCollection<KataCategoryDto>(await _kataCategoryRepository.GetAllKataCategoryAsync());
                ExecuteCancelKataCategoryCommand(obj);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji kategorii kata: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsKataCatValid(KataCategoryDto kataCategoryDto)
        {
            if (string.IsNullOrWhiteSpace(kataCategoryDto.KataCatName) ||
                kataCategoryDto.KataCatAgeMin < 0 ||
                kataCategoryDto.KataCatAgeMax <= 0 ||
                kataCategoryDto.KataCatAgeMin > kataCategoryDto.KataCatAgeMax ||
                kataCategoryDto.CatKataDegrees == null || !kataCategoryDto.CatKataDegrees.Any())
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.\n", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        #endregion

        #region KumiteCategory 
        private bool CanEditKumiteCategory(object obj) => SelectedKumiteCategory != null;
        private bool CanCancelKumiteCategory(object obj) => EditingKumiteCategory != null && SelectedKumiteCategory != null;

        private void FilterCatKumite()
        {
            KumiteCategories = string.IsNullOrWhiteSpace(SearchTextCatKumite)
                ? new ObservableCollection<KumiteCategoryDto>(_allKumiteCategories)
                : new ObservableCollection<KumiteCategoryDto>(
                    _searchService.SearchInCollection(_allKumiteCategories, SearchTextCatKumite, "KumiteCatName"));
        }
        private void ExecuteEditKumiteCategoryCommand(object obj)
        {
            if(SelectedKumiteCategory == null) return;

            EditingKumiteCategory = new KumiteCategoryDto
            {
                KumiteCatId = SelectedKumiteCategory.KumiteCatId,
                KumiteCatName = SelectedKumiteCategory.KumiteCatName,
                KumiteCatGender = SelectedKumiteCategory.KumiteCatGender,
                KumiteCatAgeMin = SelectedKumiteCategory.KumiteCatAgeMin,
                KumiteCatAgeMax = SelectedKumiteCategory.KumiteCatAgeMax,
                KumiteCatWeightMin = SelectedKumiteCategory.KumiteCatWeightMin,
                KumiteCatWeightMax = SelectedKumiteCategory.KumiteCatWeightMax
            };
            SelectedGenderCatKumite = GenderOptions.FirstOrDefault(g => g.Value == EditingKumiteCategory.KumiteCatGender);
            IsEditingExistingCatKumite = true;
        }
        private void ExecuteCancelKumiteCategoryCommand(object obj)
        {
            EditingKumiteCategory = new KumiteCategoryDto();
            SelectedGenderCatKumite = null;
            IsEditingExistingCatKumite = false;
            SelectedKumiteCategory = null;
        }
        private async void ExecuteAddKumiteCategoryCommand(object obj)
        {
            try
            {
                if (!IsKumiteCatValid(EditingKumiteCategory)) return;

                EditingKumiteCategory.KumiteCatGender = SelectedGenderCatKumite.Value;

                await _kumiteCategoryRepository.AddKumiteCategoryAsync(EditingKumiteCategory);

                _allKumiteCategories = await _kumiteCategoryRepository.GetAllKumiteCategoryAsync();
                KumiteCategories = new ObservableCollection<KumiteCategoryDto>(_allKumiteCategories);
                ExecuteCancelKumiteCategoryCommand(obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania kategorii kumite: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ExecuteUpdateKumiteCategoryCommand(object obj)
        {
            try
            {
                if (!IsKumiteCatValid(EditingKumiteCategory) || SelectedKumiteCategory == null) return;

                SelectedKumiteCategory.KumiteCatName = EditingKumiteCategory.KumiteCatName;
                SelectedKumiteCategory.KumiteCatGender = SelectedGenderCatKumite.Value;
                SelectedKumiteCategory.KumiteCatAgeMin = EditingKumiteCategory.KumiteCatAgeMin;
                SelectedKumiteCategory.KumiteCatAgeMax = EditingKumiteCategory.KumiteCatAgeMax;
                SelectedKumiteCategory.KumiteCatWeightMin = EditingKumiteCategory.KumiteCatWeightMin;
                SelectedKumiteCategory.KumiteCatWeightMax = EditingKumiteCategory.KumiteCatWeightMax;

                await _kumiteCategoryRepository.UpdateKumiteCategoryAsync(SelectedKumiteCategory);

                KumiteCategories = new ObservableCollection<KumiteCategoryDto>(await _kumiteCategoryRepository.GetAllKumiteCategoryAsync());
                ExecuteCancelKumiteCategoryCommand(obj);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas aktualizacji kategorii kumite: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsKumiteCatValid(KumiteCategoryDto kumiteCategoryDto)
        {
            if (string.IsNullOrWhiteSpace(kumiteCategoryDto.KumiteCatName) ||
                kumiteCategoryDto.KumiteCatWeightMin < 0 ||
                kumiteCategoryDto.KumiteCatWeightMax <= 0 ||
                kumiteCategoryDto.KumiteCatWeightMin > kumiteCategoryDto.KumiteCatWeightMax ||
                kumiteCategoryDto.KumiteCatAgeMin < 0 ||
                kumiteCategoryDto.KumiteCatAgeMax <= 0 ||
                kumiteCategoryDto.KumiteCatAgeMin > kumiteCategoryDto.KumiteCatAgeMax)
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.\n", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        #endregion

        #region DegreeKataCategory
        private bool CanAddDegree(object obj) => SelectedDegreeToAdd != null;
        private bool CanRemoveDegree(object obj) => SelectedCatKataDegree != null;

        private void ExecuteAddDegreeCommand(object obj)
        {
            if (SelectedDegreeToAdd == null || EditingKataCategory == null) return;

            EditingKataCategory.CatKataDegrees ??= new ObservableCollection<CatKataDegreeDto>();

            bool exists = EditingKataCategory.CatKataDegrees?
                .Any(x => x.DegreeId == SelectedDegreeToAdd.DegreeId) ?? false;

            if (!exists)
            {
                EditingKataCategory.CatKataDegrees ??= new ObservableCollection<CatKataDegreeDto>();
                EditingKataCategory.CatKataDegrees.Add(new CatKataDegreeDto
                {
                    DegreeId = SelectedDegreeToAdd.DegreeId,
                    KataCatId = EditingKataCategory.KataCatId,
                    DegreeName = SelectedDegreeToAdd.DegreeName
                });

                SelectedDegreeToAdd = null;
                OnPropertyChanged(nameof(EditingKataCategory.CatKataDegrees));
            }
        }
        private void ExecuteRemoveDegreeCommand(object obj)
        {
            if (SelectedCatKataDegree == null || EditingKataCategory == null) return;
            EditingKataCategory.CatKataDegrees.Remove(SelectedCatKataDegree);
            OnPropertyChanged(nameof(EditingKataCategory.CatKataDegrees));
        }
        #endregion
    }
}
