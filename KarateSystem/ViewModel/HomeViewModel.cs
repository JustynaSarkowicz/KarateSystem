using KarateSystem.Misc;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        #region Fields
        private string _tourName;
        private string _tourPlace;
        private string _tourDate;
        private int _competitorCount;
        private int _kataCategoryCount;
        private int _kumiteCategoryCount;
        private int _kataCount;
        private string _tourStatus;

        private ITournamentRepository _tournamentRepository;
        #endregion

        #region Properties
        public string TourStatus
        {
            get => _tourStatus;
            set
            {
                _tourStatus = value;
                OnPropertyChanged(nameof(TourStatus));
            }
        }
        public int KataCount
        {
            get => _kataCount;
            set
            {
                _kataCount = value;
                OnPropertyChanged(nameof(KataCount));
            }
        }
        public int CompetitorCount
        {
            get => _competitorCount;
            set
            {
                _competitorCount = value;
                OnPropertyChanged(nameof(CompetitorCount));
            }
        }

        public string TourName
        {
            get => _tourName;
            set
            {
                _tourName = value;
                OnPropertyChanged(nameof(TourName));
            }
        }
        public int KataCategoryCount
        {
            get => _kataCategoryCount;
            set
            {
                _kataCategoryCount = value;
                OnPropertyChanged(nameof(KataCategoryCount));
            }
        }

        public int KumiteCategoryCount
        {
            get => _kumiteCategoryCount;
            set
            {
                _kumiteCategoryCount = value;
                OnPropertyChanged(nameof(KumiteCategoryCount));
            }
        }

        public string TourPlace
        {
            get => _tourPlace;
            set
            {
                _tourPlace = value;
                OnPropertyChanged(nameof(TourPlace));
            }
        }

        public string TourDate
        {
            get => _tourDate;
            set
            {
                _tourDate = value;
                OnPropertyChanged(nameof(TourDate));
            }
        }


        #endregion
        public HomeViewModel(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
            LoadLastFinishedTournament();
        }

        public async void LoadLastFinishedTournament()
        {
            var lastTournament = await _tournamentRepository.GetLastFinishedTournament();

            if (lastTournament != null)
            {
                TourName = lastTournament.TourName;
                TourPlace = lastTournament.TourPlace;
                TourDate = lastTournament.TourDate.ToString("dd.MM.yyyy");
                TourStatus = Helper.StatusOptionsList.FirstOrDefault(x => x.Value == lastTournament.Status)?.DisplayName ?? "";

                CompetitorCount = lastTournament.TourCompetitors.Count;
                KataCategoryCount = lastTournament.TourCatKatas.Count;
                KumiteCategoryCount = lastTournament.TourCatKumites.Count;
                KataCount = lastTournament.TourCompetitors.Where(t => t.KataId != null).Count();
            }
            else
            {
                TourName = "Brak";
                TourPlace = "Brak";
                TourDate = "-";

                CompetitorCount = 0;
                KataCategoryCount = 0;
                KumiteCategoryCount = 0;
                KataCount = 0;
                TourStatus = "";
            }
        }


    }
}
