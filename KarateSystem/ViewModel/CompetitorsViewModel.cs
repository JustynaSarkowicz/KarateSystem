using Accessibility;
using AutoMapper;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using System.Collections.ObjectModel;
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
        private DegreeDto _selectedDegree;
        private ClubDto _selectedClub;

        private ObservableCollection<CompetitorDto> _competitors;
        private ObservableCollection<DegreeDto> _degrees;
        private ObservableCollection<ClubDto> _clubs;
        private List<CompetitorDto> _allCompetitors;
        
        private readonly ICompetitorRepository _competitorRepository;
        private readonly IDegreeRepository _degreeRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ISearchService _searchService;
        #endregion

        #region Commands
        public ICommand EditCompCommand { get; }
        public ICommand CancelCompCommand { get; }
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
        public DegreeDto SelectedDegree
        {
            get => _selectedDegree;
            set
            {
                _selectedDegree = value;
                OnPropertyChanged(nameof(SelectedDegree));
            }
        }
        public ClubDto SelectedClub
        {
            get => _selectedClub;
            set
            {
                _selectedClub = value;
                OnPropertyChanged(nameof(SelectedClub));
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

            _ = LoadAsync();
        }
        private bool CanEditCompetitor(object obj) => SelectedCompetitor != null;
        private async Task LoadAsync()
        {
            _allCompetitors = await _competitorRepository.GetAllCompetitorsAsync();
            Competitors = new ObservableCollection<CompetitorDto>(_allCompetitors);

            var degrees = await _degreeRepository.GetAllDegreeAsync();
            Degrees = new ObservableCollection<DegreeDto>(degrees);

            var clubs = await _clubRepository.GetAllClubsAsync();
            Clubs = new ObservableCollection<ClubDto>(clubs);
            EditingComp = new CompetitorDto();
        }
        private void FilterComp()
        {
            Competitors = string.IsNullOrWhiteSpace(SearchTextComp)
                ? new ObservableCollection<CompetitorDto>(_allCompetitors)
                : new ObservableCollection<CompetitorDto>(
                    _searchService.SearchInCollection(_allCompetitors, SearchTextComp, "CompFirstName", "CompLastName"));
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
                CompClubId = SelectedCompetitor.CompClubId
            };
            SelectedGenderComp = GenderOptions2.FirstOrDefault(g => g.Value == EditingComp.CompGender);
            SelectedDegree = Degrees.FirstOrDefault(d => d.DegreeId == EditingComp.CompDegreeId);
            SelectedClub = Clubs.FirstOrDefault(c => c.ClubId == EditingComp.CompClubId);
            IsEditingExisting = true;            
        }
        private void ExecuteCancelCompCommand(object obj)
        {
            EditingComp = new CompetitorDto();
            SelectedGenderComp = null;
            SelectedDegree = null;
            SelectedClub = null;
            IsEditingExisting = false;
            SelectedCompetitor = null;
        }
    }
}
