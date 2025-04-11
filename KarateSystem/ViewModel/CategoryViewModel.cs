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
        private GenderOption? _selectedGenderCatKata;
        private GenderOption2 _selectedGenderCatKumite;
        private KumiteCategoryDto _selectedKumiteCategory;
        private KumiteCategoryDto _editingKumiteCategory;

        private ObservableCollection<KataCategoryDto> _kataCategories;
        private ObservableCollection<DegreeDto> _degrees;
        private List<KataCategoryDto> _allKataCategories;
        private ObservableCollection<KumiteCategoryDto> _kumiteCategories;
        private List<KumiteCategoryDto> _allKumiteCategories;

        private readonly ICataCategoryRepository _cataCategoryRepository;
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
        public GenderOption? SelectedGenderCatKata
        {
            get => _selectedGenderCatKata;
            set
            {
                _selectedGenderCatKata = value;
                OnPropertyChanged(nameof(SelectedGenderCatKata));
            }
        }
        public GenderOption2 SelectedGenderCatKumite
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
        public CategoryViewModel(ICataCategoryRepository cataCategoryRepository,
            IDegreeRepository degreeRepository,
            ICatKataDegreeRepository catKataDegreeRepository,
            ISearchService searchService,
            IKumiteCategoryRepository kumiteCategoryRepository)
        {
            _cataCategoryRepository = cataCategoryRepository;
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

            _ = LoadAsync();
        }
        private async Task LoadAsync()
        {
            _allKataCategories = await _cataCategoryRepository.GetAllKataCategoryAsync();
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
        private bool CanEditKataCategory(object obj) => SelectedKataCategory != null;
        private bool CanCancelKataCategory(object obj) => EditingKataCategory != null && SelectedKataCategory != null;
        private bool CanEditKumiteCategory(object obj) => SelectedKumiteCategory != null;
        private bool CanCancelKumiteCategory(object obj) => EditingKumiteCategory != null && SelectedKumiteCategory != null;
        private bool CanAddDegree(object obj) => SelectedDegreeToAdd != null;
        private bool CanRemoveDegree(object obj) => SelectedCatKataDegree != null;

        #region KataCategory
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
            SelectedGenderCatKata = GenderOptions.FirstOrDefault(g => g.Value == EditingKataCategory.KataCatGender);
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
            if (EditingKataCategory == null)
                return;

            var result = await _cataCategoryRepository.AddKataCategoryAsync(EditingKataCategory);

            if (!result)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione:\n- Nazwa\n- Zakres wiekowy (min <= max)\n- Płeć\n- Przynajmniej jeden stopień",
                                "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedList = await _cataCategoryRepository.GetAllKataCategoryAsync();
            KataCategories = new ObservableCollection<KataCategoryDto>(updatedList);
            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
            SelectedGenderCatKata = GenderOptions.FirstOrDefault(g => g.Value == null);
            SelectedDegreeToAdd = null;
            IsEditingExistingCatKata = false;
        }
        private async void ExecuteUpdateKataCategoryCommand(object obj)
        {
            if (SelectedKataCategory == null || EditingKataCategory == null)
                return;
            if (!KataCategories.Contains(SelectedKataCategory)) return;
            SelectedKataCategory.KataCatName = EditingKataCategory.KataCatName;
            SelectedKataCategory.KataCatGender = EditingKataCategory.KataCatGender;
            SelectedKataCategory.KataCatAgeMin = EditingKataCategory.KataCatAgeMin;
            SelectedKataCategory.KataCatAgeMax = EditingKataCategory.KataCatAgeMax;
            SelectedKataCategory.CatKataDegrees = EditingKataCategory.CatKataDegrees;
            var succeed = await _cataCategoryRepository.UpdateKataCategoryAsync(SelectedKataCategory);
            if (!succeed)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione:\n- Nazwa\n- Zakres wiekowy (min <= max)\n- Przynajmniej jeden stopień", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            KataCategories = new ObservableCollection<KataCategoryDto>(_allKataCategories);
            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
            SelectedGenderCatKata = GenderOptions.FirstOrDefault(g => g.Value == null);
            IsEditingExistingCatKata = false;
        }
        #endregion

        #region KumiteCategory
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
            SelectedGenderCatKumite = GenderOptions2.FirstOrDefault(g => g.Value == EditingKumiteCategory.KumiteCatGender);
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
            if (EditingKumiteCategory == null)
                return;

            var result = await _kumiteCategoryRepository.AddKumiteCategoryAsync(EditingKumiteCategory);

            if (!result)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione:\n- Nazwa\n- Zakres wiekowy (min <= max)\n- Płeć\n- Zaker wagowy (min <= max)",
                                "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedList = await _kumiteCategoryRepository.GetAllKumiteCategoryAsync();
            KumiteCategories = new ObservableCollection<KumiteCategoryDto>(updatedList);
            EditingKumiteCategory = new KumiteCategoryDto();
            SelectedGenderCatKumite = GenderOptions2.FirstOrDefault(g => g.Value == true);
            IsEditingExistingCatKumite = false;
        }
        private async void ExecuteUpdateKumiteCategoryCommand(object obj)
        {
            if(SelectedKumiteCategory == null || EditingKumiteCategory == null)
                return;
            if (!KumiteCategories.Contains(SelectedKumiteCategory)) return;
            SelectedKumiteCategory.KumiteCatName = EditingKumiteCategory.KumiteCatName;
            SelectedKumiteCategory.KumiteCatGender = EditingKumiteCategory.KumiteCatGender;
            SelectedKumiteCategory.KumiteCatAgeMin = EditingKumiteCategory.KumiteCatAgeMin;
            SelectedKumiteCategory.KumiteCatAgeMax = EditingKumiteCategory.KumiteCatAgeMax;
            SelectedKumiteCategory.KumiteCatWeightMin = EditingKumiteCategory.KumiteCatWeightMin;
            SelectedKumiteCategory.KumiteCatWeightMax = EditingKumiteCategory.KumiteCatWeightMax;
            var succeed = await _kumiteCategoryRepository.UpdateKumiteCategoryAsync(SelectedKumiteCategory);
            if (!succeed)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione:\n- Nazwa\n- Zakres wiekowy (min <= max)\n- Przynajmniej jeden stopień", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            KumiteCategories = new ObservableCollection<KumiteCategoryDto>(_allKumiteCategories);
            EditingKumiteCategory = new KumiteCategoryDto();
            SelectedGenderCatKumite = GenderOptions2.FirstOrDefault(g => g.Value == true);
            IsEditingExistingCatKumite = false;
        }
        #endregion

        #region DegreeKataCategory
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
