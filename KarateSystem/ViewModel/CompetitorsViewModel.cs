using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using System.Collections.ObjectModel;
using static KarateSystem.Misc.Enum;

namespace KarateSystem.ViewModel
{
    public class CompetitorsViewModel : ViewModelBase
    {
        private Competitor _selectedComp;
        private ObservableCollection<Competitor> _competitors;
        private ObservableCollection<Tournament> _tournaments;

        private readonly ICompetitorRepository _competitorRepository;

        public List<Gender> Genders => Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
        public ObservableCollection<Competitor> Competitors
        {
            get => _competitors;
            set
            {
                _competitors = value;
                OnPropertyChanged(nameof(Competitors));
            }
        }
        public ObservableCollection<Tournament> Tournaments
        {
            get => _tournaments;
            set
            {
                _tournaments = value;
                OnPropertyChanged(nameof(Tournaments));
            }
        }
        public Competitor SelectedCompetitor
        {
            get => _selectedComp;
            set
            {
                if (_selectedComp != value)
                {
                    _selectedComp = value;
                    OnPropertyChanged(nameof(SelectedCompetitor)); 
                }
            }
        }
        public CompetitorsViewModel(ICompetitorRepository competitorRepository)
        {
            _competitorRepository = competitorRepository;
            _competitors = new ObservableCollection<Competitor>(competitorRepository.GetAllCompetitors());
            _tournaments = new ObservableCollection<Tournament>();
        }
    }
}
