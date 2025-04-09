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
using static System.Reflection.Metadata.BlobBuilder;

namespace KarateSystem.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        #region Fields
        private KataCategoryDto _selectedKataCategory;
        private KataCategoryDto _editingKataCategory;
        private DegreeDto _selectedDegreeToAdd;
        private CatKataDegreeDto _selectedCatKataDegree;
        private GenderOption? _selectedGender;
        private bool _isEditingExisting;
        private ObservableCollection<KataCategoryDto> _kataCategories;
        private ObservableCollection<DegreeDto> _degrees;

        private readonly ICataCategoryRepository _cataCategoryRepository;
        private readonly IDegreeRepository _degreeRepository;
        private readonly ICatKataDegreeRepository _catKataDegreeRepository;
        #endregion

        #region Commands
        public ICommand EditKataCategoryCommand { get; }
        public ICommand CancelKataCategoryCommand { get; }
        public ICommand UpdateKataCategoryCommand { get; }
        public ICommand AddKataCategoryCommand { get; }
        public ICommand AddDegreeCommand { get; }
        public ICommand RemoveDegreeCommand { get; }
        #endregion
        //Properties
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
        public KataCategoryDto? SelectedKataCategory
        {
            get => _selectedKataCategory;
            set
            {
                _selectedKataCategory = value;
                OnPropertyChanged(nameof(SelectedKataCategory));
            }
        }
        public DegreeDto? SelectedDegreeToAdd
        {
            get => _selectedDegreeToAdd;
            set
            {
                _selectedDegreeToAdd = value;
                OnPropertyChanged(nameof(SelectedDegreeToAdd));
            }
        }
        public CatKataDegreeDto? SelectedCatKataDegree
        {
            get => _selectedCatKataDegree;
            set
            {
                _selectedCatKataDegree = value;
                OnPropertyChanged(nameof(SelectedCatKataDegree));
            }
        }
        public KataCategoryDto? EditingKataCategory
        {
            get => _editingKataCategory;
            set
            {
                _editingKataCategory = value;
                OnPropertyChanged(nameof(EditingKataCategory));
                OnPropertyChanged(nameof(EditingKataCategory.CatKataDegrees));
            }
        }
        public GenderOption SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
                if (EditingKataCategory != null)
                {
                    EditingKataCategory.KataCatGender = value.Value;
                    OnPropertyChanged(nameof(EditingKataCategory));
                }
            }
        }
        public CategoryViewModel(ICataCategoryRepository cataCategoryRepository,
            IDegreeRepository degreeRepository,
            ICatKataDegreeRepository catKataDegreeRepository)
        {
            _cataCategoryRepository = cataCategoryRepository;
            _degreeRepository = degreeRepository;
            _catKataDegreeRepository = catKataDegreeRepository;


            EditKataCategoryCommand = new ViewModelCommand(ExecuteEditKataCategoryCommand, CanEditKataCategory);
            CancelKataCategoryCommand = new ViewModelCommand(ExecuteCancelKataCategoryCommand);
            UpdateKataCategoryCommand = new ViewModelCommand(ExecuteUpdateKataCategoryCommand, CanCancelKataCategory);
            AddKataCategoryCommand = new ViewModelCommand(ExecuteAddKataCategoryCommand);
            AddDegreeCommand = new ViewModelCommand(ExecuteAddDegreeCommand, CanAddDegree);
            RemoveDegreeCommand = new ViewModelCommand(ExecuteRemoveDegreeCommand, CanRemoveDegree);

            _ = LoadAsync();
        }
        private async Task LoadAsync()
        {
            var categories = await _cataCategoryRepository.GetAllKataCategoryAsync();
            KataCategories = new ObservableCollection<KataCategoryDto>(categories);

            var degrees = await _degreeRepository.GetAllDegreeAsync();
            Degrees = new ObservableCollection<DegreeDto>(degrees);

            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
        }
        private bool CanEditKataCategory(object obj) => SelectedKataCategory != null;
        private bool CanCancelKataCategory(object obj) => EditingKataCategory != null && SelectedKataCategory != null;
        private bool CanAddDegree(object obj) => SelectedDegreeToAdd != null;
        private bool CanRemoveDegree(object obj) => SelectedCatKataDegree != null;
        private void ExecuteEditKataCategoryCommand(object obj)
        {
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
            SelectedGender = GenderOptions.FirstOrDefault(g => g.Value == EditingKataCategory.KataCatGender);
            IsEditingExisting = true;
        }
        private void ExecuteCancelKataCategoryCommand(object obj)
        {
            EditingKataCategory = new KataCategoryDto();
            SelectedGender = GenderOptions.FirstOrDefault(g => g.Value == null);
            IsEditingExisting = false;
            SelectedKataCategory = null;
        }
        private async void ExecuteAddKataCategoryCommand(object obj)
        {
            if (EditingKataCategory == null)
                return;

            var result = await _cataCategoryRepository.AddKataCategoryAsync(EditingKataCategory);

            if (!result)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione:\n- Nazwa\n- Zakres wiekowy (min <= max)\n- Przynajmniej jeden stopień",
                                "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedList = await _cataCategoryRepository.GetAllKataCategoryAsync();
            KataCategories = new ObservableCollection<KataCategoryDto>(updatedList);
            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
            SelectedGender = GenderOptions.FirstOrDefault(g => g.Value == null);
            SelectedDegreeToAdd = null;
            IsEditingExisting = false;
        }

        private async void ExecuteUpdateKataCategoryCommand(object obj)
        {
            if (!KataCategories.Contains(SelectedKataCategory)) return;
            SelectedKataCategory.KataCatName = EditingKataCategory.KataCatName;
            SelectedKataCategory.KataCatGender = EditingKataCategory.KataCatGender;
            SelectedKataCategory.KataCatAgeMin = EditingKataCategory.KataCatAgeMin;
            SelectedKataCategory.KataCatAgeMax = EditingKataCategory.KataCatAgeMax;
            SelectedKataCategory.CatKataDegrees = EditingKataCategory.CatKataDegrees;
            var succeed = await _cataCategoryRepository.UpdateKataCategoryAsync(SelectedKataCategory);
            var index = KataCategories.IndexOf(SelectedKataCategory);
            KataCategories[index] = SelectedKataCategory;

            if (!succeed)
            {
                MessageBox.Show("Upewnij się, że wszystkie pola są poprawnie wypełnione:\n- Nazwa\n- Zakres wiekowy (min <= max)\n- Przynajmniej jeden stopień", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var updatedKataCategory = new ObservableCollection<KataCategoryDto>(KataCategories);
            KataCategories = updatedKataCategory;
            EditingKataCategory = new KataCategoryDto
            {
                CatKataDegrees = new ObservableCollection<CatKataDegreeDto>()
            };
            SelectedGender = GenderOptions.FirstOrDefault(g => g.Value == null);
            IsEditingExisting = false;
        }
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
            EditingKataCategory.CatKataDegrees.Remove(SelectedCatKataDegree);
            OnPropertyChanged(nameof(EditingKataCategory.CatKataDegrees));
        }
    }
}
