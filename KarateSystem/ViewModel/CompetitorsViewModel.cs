using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using System.Collections.ObjectModel;
using static KarateSystem.Misc.Enum;

namespace KarateSystem.ViewModel
{
    public class CompetitorsViewModel : ViewModelBase
    {
        private Competitor selectedComp;
        private ObservableCollection<Competitor> competitors;
        private ObservableCollection<Tournament> tournaments;

        private readonly ICompetitorRepository _competitorRepository;

        public List<Gender> Genders => Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
        public ObservableCollection<Competitor> Competitors
        {
            get => competitors;
            set
            {
                competitors = value;
                OnPropertyChanged(nameof(Competitors));
            }
        }
        public ObservableCollection<Tournament> Tournaments
        {
            get => tournaments;
            set
            {
                tournaments = value;
                OnPropertyChanged(nameof(Tournaments));
            }
        }
        public Competitor SelectedCompetitor
        {
            get => selectedComp;
            set
            {
                if (selectedComp != value)
                {
                    selectedComp = value;
                    OnPropertyChanged(nameof(SelectedCompetitor)); 
                }
            }
        }
        public CompetitorsViewModel(ICompetitorRepository competitorRepository)
        {
            _competitorRepository = competitorRepository;
            LoadCompetitorsAsync();
        }
        private async void LoadCompetitorsAsync()
        {
            var allCompetitors = await _competitorRepository.GetAllCompetitorsAsync();
            Competitors = new ObservableCollection<Competitor>(allCompetitors);
        }
    }
}
