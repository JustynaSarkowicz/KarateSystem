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
    public class SetCompToCatTourViewModel : ViewModelBase
    {
        #region Fields
        public int TourId { get; set; }
        private string _searchTextComp;
        private string _selectedFilterType;
        private string _selectedFilterValue;
        private ObservableCollection<TourCompetitorDto> _competitors;
        private List<TourCompetitorDto> _allCompetitors;
        private readonly ITourCompetitorRepository _tourCompetitorRepository;
        private readonly ISearchService _searchService;
        #endregion

        #region Commands
        public ICommand AddCompCommand { get; }
        public ICommand FilterCommand { get; }
        #endregion

        #region Properties
        public List<TourCompetitorDto> SelectedCompetitors { get; set; } = new();
        public ObservableCollection<string> FilterTypes { get; set; } = new() { "", "Płeć", "Stopień", "Klub" };
        public ObservableCollection<string> FilterValues { get; set; } = new();
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
        public ObservableCollection<TourCompetitorDto> Competitors
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
        public SetCompToCatTourViewModel(ITourCompetitorRepository tourCompetitorRepository,
            ISearchService searchService)
        {
            _searchService = searchService;
            _tourCompetitorRepository = tourCompetitorRepository;

            FilterCommand = new ViewModelCommand(ExecuteFilter);
            AddCompCommand = new ViewModelCommand(AddSelected);
        }
        public async Task LoadAsync()
        {
            _allCompetitors = await _tourCompetitorRepository.GetTourCompetitorsByIdTourAsync(TourId);
            Competitors = new ObservableCollection<TourCompetitorDto>(_allCompetitors);
        }
        private void AddSelected(object parameter)
        {
            if (parameter is Window window && window is SetCompToCatTourView view)
            {
                var selected = view.dgvCompetitors.SelectedItems
                    .Cast<TourCompetitorDto>()
                    .ToList();

                SelectedCompetitors = selected;
                window.DialogResult = true;
                window.Close();
            }
        }

        private void FilterComp()
        {
            Competitors = string.IsNullOrWhiteSpace(SearchTextComp)
                ? new ObservableCollection<TourCompetitorDto>(_allCompetitors)
                : new ObservableCollection<TourCompetitorDto>(
                    _searchService.SearchInCollection(_allCompetitors, SearchTextComp, "CompFirstName", "CompLastName"));
        }
        private void ExecuteFilter(object obj)
        {
            if (string.IsNullOrEmpty(SelectedFilterType) || string.IsNullOrEmpty(SelectedFilterValue) || SelectedFilterType == "")
            {
                Competitors = new ObservableCollection<TourCompetitorDto>(_allCompetitors);
                return;
            }

            IEnumerable<TourCompetitorDto> filtered = _allCompetitors;

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

            Competitors = new ObservableCollection<TourCompetitorDto>(filtered);
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
