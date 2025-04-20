using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using KarateSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KarateSystem.ViewModel
{
    public class AddCompetitorsViewModel : ViewModelBase
    {
        #region Fields
        private string _searchTextComp;
        private string _selectedFilterType;
        private string _selectedFilterValue;
        private ObservableCollection<CompetitorDto> _competitors;
        private List<CompetitorDto> _allCompetitors;
        public List<CompetitorDto> SelectedCompetitors { get; set; } = new();
        public ObservableCollection<string> FilterTypes { get; set; } = new() { "", "Płeć", "Stopień", "Klub" };
        public ObservableCollection<string> FilterValues { get; set; } = new();
        private readonly ICompetitorRepository _competitorRepository;
        private readonly ISearchService _searchService;
        #endregion

        #region Commands
        public ICommand AddCompCommand { get; }
        public ICommand FilterCommand { get; }
        #endregion

        #region Properties
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
        public ObservableCollection<CompetitorDto> Competitors
        {
            get => _competitors;
            set
            {
                _competitors = value;
                OnPropertyChanged(nameof(Competitors));
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
        public AddCompetitorsViewModel(ICompetitorRepository competitorRepository, 
            ISearchService searchService)
        {
            _competitorRepository = competitorRepository;
            _searchService = searchService;

            FilterCommand = new ViewModelCommand(ExecuteFilter);
            AddCompCommand = new ViewModelCommand(AddSelected);

            LoadAsync();
        }
        private async void LoadAsync()
        {
            _allCompetitors = await _competitorRepository.GetAllCompetitorsAsync();
            Competitors = new ObservableCollection<CompetitorDto>(_allCompetitors);
        }
        private void AddSelected(object parameter)
        {
            if (parameter is Window window && window is AddCompetitorsView view)
            {
                var selected = view.dgvCompetitors.SelectedItems
                    .Cast<CompetitorDto>()
                    .ToList();

                SelectedCompetitors = selected;
                window.DialogResult = true;
                window.Close();
            }
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
