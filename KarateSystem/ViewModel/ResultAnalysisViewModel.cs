using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KarateSystem.ViewModel
{
    public class ResultAnalysisViewModel : ViewModelBase
    {
        #region Fields

        private TournamentDto _selectedTour;
        private ObservableCollection<TournamentDto> _tournaments;

        private PlotModel _medalsByClubPlot;
        private PlotModel _clubParticipationPlot;
        private PlotModel _genderDistributionPlot;
        private PlotModel _ageDistributionPlot;
        private PlotModel _fightDurationPlot;
        private PlotModel _fightsPerCategoryPlot;
        private PlotModel _katasPerCategoryPlot;
        private PlotModel _katasAverageScorePlot;

        private readonly ITournamentRepository _tournamentRepository;
        private readonly IResultStatsService _resultStatsService;
        #endregion

        #region Properties
        public PlotModel KatasAverageScorePlot
        {
            get => _katasAverageScorePlot;
            set
            {
                if (_katasAverageScorePlot != value)
                {
                    _katasAverageScorePlot = value;
                    OnPropertyChanged(nameof(KatasAverageScorePlot));
                }
            }
        }
        public PlotModel KatasPerCategoryPlot
        {
            get => _katasPerCategoryPlot;
            set
            {
                if (_katasPerCategoryPlot != value)
                {
                    _katasPerCategoryPlot = value;
                    OnPropertyChanged(nameof(KatasPerCategoryPlot));
                }
            }
        }
        public PlotModel ClubParticipationPlot
        {
            get => _clubParticipationPlot;
            set
            {
                if (_clubParticipationPlot != value)
                {
                    _clubParticipationPlot = value;
                    OnPropertyChanged(nameof(ClubParticipationPlot));
                }
            }
        }
        public PlotModel GenderDistributionPlot
        {
            get => _genderDistributionPlot;
            set
            {
                if (_genderDistributionPlot != value)
                {
                    _genderDistributionPlot = value;
                    OnPropertyChanged(nameof(GenderDistributionPlot));
                }
            }
        }
        public PlotModel AgeDistributionPlot
        {
            get => _ageDistributionPlot;
            set
            {
                if (_ageDistributionPlot != value)
                {
                    _ageDistributionPlot = value;
                    OnPropertyChanged(nameof(AgeDistributionPlot));
                }
            }
        }
        public PlotModel FightDurationPlot
        {
            get => _fightDurationPlot;
            set
            {
                if (_fightDurationPlot != value)
                {
                    _fightDurationPlot = value;
                    OnPropertyChanged(nameof(FightDurationPlot));
                }
            }
        }
        public PlotModel FightsPerCategoryPlot
        {
            get => _fightsPerCategoryPlot;
            set
            {
                if (_fightsPerCategoryPlot != value)
                {
                    _fightsPerCategoryPlot = value;
                    OnPropertyChanged(nameof(FightsPerCategoryPlot));
                }
            }
        }
        public PlotModel ClubParticipationPlot2
        {
            get => _clubParticipationPlot;
            set
            {
                if (_clubParticipationPlot != value)
                {
                    _clubParticipationPlot = value;
                    OnPropertyChanged(nameof(ClubParticipationPlot2));
                }
            }
        }
        public PlotModel MedalsByClubPlot
        {
            get => _medalsByClubPlot;
            set
            {
                if (_medalsByClubPlot != value)
                {
                    _medalsByClubPlot = value;
                    OnPropertyChanged(nameof(MedalsByClubPlot));
                }
            }
        }
        public TournamentDto SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }
        public ObservableCollection<TournamentDto> Tournaments
        {
            get => _tournaments;
            set
            {
                if (_tournaments != value)
                {
                    _tournaments = value;
                    OnPropertyChanged(nameof(Tournaments));
                }
            }
        }
        #endregion

        #region Commands
        public ICommand LoadStatisticsCommand { get; }
        #endregion

        public ResultAnalysisViewModel(ITournamentRepository tournamentRepository,
            IResultStatsService resultStatsService)
        {
            _tournamentRepository = tournamentRepository;
            _resultStatsService = resultStatsService;

            LoadStatisticsCommand = new ViewModelCommand(async (_) => await LoadStatisticsAsync());

            _ = LoadAsync();
        }

        public async Task LoadAsync()
        {
            var tournaments = await _tournamentRepository.GetOnlyActiveTournamentsAsync().ConfigureAwait(false);
            Tournaments = new ObservableCollection<TournamentDto>(tournaments);
        }

        public async Task LoadStatisticsAsync()
        {
            if (SelectedTour == null)
                return;

            MedalsByClubPlot = await CreateMedalsChartAsync();
            ClubParticipationPlot = await CreateClubParticipationChartAsync();
            GenderDistributionPlot = await CreateGenderDistributionChartAsync();
            AgeDistributionPlot = await CreateAgeDistributionChart();
            FightDurationPlot = await CreateFightDurationChartAsync();
            FightsPerCategoryPlot = await CreateFightsPerCategoryChartAsync();
            KatasPerCategoryPlot = await CreateCompetitorsPerKataCategoryChartAsync();
            KatasAverageScorePlot = await CreateKataScoreDistributionChartAsync();
        }

        private async Task<PlotModel> CreateMedalsChartAsync()
        {
            if (SelectedTour == null)
                return null;

            var medalStats = await _resultStatsService.GetMedalStatsByClubAsync(SelectedTour.TourId);

            var model = new PlotModel
            {
                Title = "Medale wg klubów",
                Background = OxyColors.Transparent,
                TitleColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White
            };

            var categories = new List<string>();
            var goldSeries = new BarSeries
            {
                Title = "Złoto",
                FillColor = OxyColors.Gold,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                BarWidth = 2,
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0}",
                TextColor = OxyColors.White,
                LabelMargin = 3
            };
            var silverSeries = new BarSeries
            {
                Title = "Srebro",
                FillColor = OxyColors.Silver,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                BarWidth = 2,
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0}",
                TextColor = OxyColors.White,
                LabelMargin = 3
            };
            var bronzeSeries = new BarSeries
            {
                Title = "Brąz",
                FillColor = OxyColor.FromRgb(205, 127, 50),
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                BarWidth = 2,
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0}",
                TextColor = OxyColors.White,
                LabelMargin = 3
            };
            var sortedClubs = medalStats
                            .OrderBy(x => x.Value[0])
                            .ToList();

            foreach (var (club, medals) in sortedClubs)
            {
                categories.Add(club);
                goldSeries.Items.Add(new BarItem { Value = medals[0] });
                silverSeries.Items.Add(new BarItem { Value = medals[1] });
                bronzeSeries.Items.Add(new BarItem { Value = medals[2] });
            }

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,  
                Key = "ClubAxis",
                ItemsSource = categories,
                TextColor = OxyColors.White, 
                TitleColor = OxyColors.White, 
                TicklineColor = OxyColors.White,
                GapWidth = 0.1,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,  
                Minimum = 0,
                Title = "Liczba medali",
                TextColor = OxyColors.White,
                TitleColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Series.Add(goldSeries);
            model.Series.Add(silverSeries);
            model.Series.Add(bronzeSeries);

            return model;
        }
        private async Task<PlotModel> CreateClubParticipationChartAsync()
        {
            var participation = await _resultStatsService.GetClubParticipationAsync(SelectedTour.TourId);
            var totalParticipants = participation.Sum(x => x.Value);

            var model = new PlotModel
            {
                Title = "Udział klubów",
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
            };

            var countSeries = new BarSeries
            {
                Title = "Liczba członków",
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0}",
                TextColor = OxyColors.White,
                FillColor = OxyColor.FromRgb(70, 130, 180), 
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                LabelMargin = 3
            };

            var percentSeries = new BarSeries
            {
                Title = "Procent udziału",
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:0.0}%",
                TextColor = OxyColors.White,
                FillColor = OxyColor.FromRgb(100, 180, 180), 
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                LabelMargin = 3
            };

            var categories = new List<string>();

            foreach (var club in participation.OrderBy(x => x.Value))
            {
                categories.Add(club.Key);
                countSeries.Items.Add(new BarItem { Value = club.Value });
                percentSeries.Items.Add(new BarItem { Value = (club.Value * 100.0) / totalParticipants });
            }

            model.Series.Add(countSeries);
            model.Series.Add(percentSeries);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = categories,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                GapWidth = 0.4,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                Title = "Wartości",
                TitleColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            return model;
        }
        private async Task<PlotModel> CreateGenderDistributionChartAsync()
        {
            var (maleCount, femaleCount) = await _resultStatsService.GetGenderDistributionAsync(SelectedTour.TourId);

            var model = new PlotModel 
            { 
                Title = "Podział płci w turnieju",
                TitleColor = OxyColors.White,
                PlotMargins = new OxyThickness(20, 20, 20, 20),
            };

            var totalCount = maleCount + femaleCount;

            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                Stroke = OxyColors.White,
                InsideLabelPosition = 0.6,
                AngleSpan = 360,
                StartAngle = 270, 
                OutsideLabelFormat = "{1}", 
                InsideLabelFormat = "{2:0.0}%", 
                TextColor = OxyColors.White,
                FontSize = 12,
                TickHorizontalLength = 15, 
                TickRadialLength = 10,
                AreInsideLabelsAngled = false, 
                ExplodedDistance = 0.05 
            };

            pieSeries.Slices.Add(new PieSlice("Mężczyźni", maleCount)
            {
                Fill = OxyColor.FromRgb(100, 180, 180),
            });

            pieSeries.Slices.Add(new PieSlice("Kobiety", femaleCount)
            {
                Fill = OxyColor.FromRgb(70, 130, 180),
            });

            model.Series.Add(pieSeries);
            return model;
        }
        private async Task<PlotModel> CreateAgeDistributionChart()
        {
            var ages = await _resultStatsService.GetCompetitorsAgesAsync(SelectedTour.TourId);
            var model = new PlotModel 
            { 
                Title = "Rozkład wieku",
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
            };

            var histogram = new HistogramSeries
            {
                FillColor = OxyColor.FromRgb(70, 130, 180),
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                TextColor = OxyColors.White,
            };

            // Grupowanie wieku w przedziały 3-letnie
            foreach (var group in ages.GroupBy(a => a / 3))
            {
                histogram.Items.Add(new HistogramItem(
                    group.Key * 3,
                    (group.Key + 1) * 3,
                    group.Count(),
                    group.Count()));
            }

            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Bottom, 
                Title = "Wiek", 
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Minimum = 0, 
                Title = "Liczba", 
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
            model.Series.Add(histogram);
            return model;
        }
        private async Task<PlotModel> CreateFightDurationChartAsync()
        {
            var durations = await _resultStatsService.GetFightDurationsAsync(SelectedTour.TourId);

            var model = new PlotModel
            {
                Title = "Czas trwania walk",
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
                TextColor = OxyColors.White
            };

            // Konwersja sekund na minuty i zaokrąglenie
            var minutes = durations.Select(sec => Math.Round(sec / 60.0, 1)).ToList();

            var histogram = new HistogramSeries
            {
                FillColor = OxyColor.FromRgb(70, 130, 180), 
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                TextColor = OxyColors.White
            };

            // Grupowanie w przedziały 1-minutowe
            foreach (var group in minutes.GroupBy(m => (int)m))
            {
                histogram.Items.Add(new HistogramItem(
                    group.Key,
                    group.Key + 1,
                    group.Count(),
                    group.Count()));
            }

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Czas (minuty)",
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Liczba walk",
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                Minimum = 0,
                TicklineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Series.Add(histogram);
            return model;
        }
        private async Task<PlotModel> CreateFightsPerCategoryChartAsync()
        {
            var fightsPerCategory = await _resultStatsService.GetFightsPerCategoryAsync(SelectedTour.TourId);

            var model = new PlotModel
            {
                Title = "Liczba walk w kategoriach kumite",
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
                TextColor = OxyColors.White,
            };

            var barSeries = new BarSeries
            {
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0}", 
                TextColor = OxyColors.White,
                FillColor = OxyColor.FromRgb(70, 130, 180), 
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                BarWidth = 0.6 
            };

            var categories = fightsPerCategory
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            foreach (var category in categories)
            {
                barSeries.Items.Add(new BarItem { Value = fightsPerCategory[category] });
            }

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = categories,
                Key = "Categories",
                TextColor = OxyColors.White,
                TitleColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                GapWidth = 0.4,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Title = "Liczba walk",
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Series.Add(barSeries);
            return model;
        }
        private async Task<PlotModel> CreateCompetitorsPerKataCategoryChartAsync()
        {
            var competitorsPerCategory = await _resultStatsService.GetKatasPerCategoryAsync(SelectedTour.TourId);

            var model = new PlotModel
            {
                Title = "Liczba zawodników w kategoriach kata",
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
                TextColor = OxyColors.White,
            };

            var barSeries = new BarSeries
            {
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0}",
                TextColor = OxyColors.White,
                FillColor = OxyColor.FromRgb(70, 130, 180), 
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                BarWidth = 0.6
            };

            var categories = competitorsPerCategory
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            foreach (var category in categories)
            {
                barSeries.Items.Add(new BarItem { Value = competitorsPerCategory[category] });
            }

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = categories,
                TextColor = OxyColors.White,
                TitleColor = OxyColors.White,
                TicklineColor = OxyColors.White,
                GapWidth = 0.4,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Title = "Liczba zawodników",
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.White,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            model.Series.Add(barSeries);
            return model;
        }
        private async Task<PlotModel> CreateKataScoreDistributionChartAsync()
        {
            var scores = await _resultStatsService.GetScoresInKatasAsync(SelectedTour.TourId);
            var doubleScores = scores.Select(s => Convert.ToDouble(s)).ToList(); 

            var model = new PlotModel
            {
                Title = "Rozkład ocen kata",
                TitleColor = OxyColors.White,
                Background = OxyColors.Transparent,
                PlotAreaBorderColor = OxyColors.White,
                TextColor = OxyColors.White
            };

            var histogram = new HistogramSeries
            {
                FillColor = OxyColor.FromRgb(70, 130, 180),
                StrokeColor = OxyColors.White,
                StrokeThickness = 1,
                TextColor = OxyColors.Black
            };

            if (doubleScores.Any())
            {
                double min = doubleScores.Min();
                double max = doubleScores.Max();

                // Przedziały co 0.5 punktu
                for (double bin = Math.Floor(min * 2) / 2; bin <= max; bin += 0.5)
                {
                    int count = doubleScores.Count(s => s >= bin && s < bin + 0.5);
                    histogram.Items.Add(new HistogramItem(bin, bin + 0.5, count, count));
                }

                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Ocena",
                    TitleColor = OxyColors.White,
                    TextColor = OxyColors.White,
                    TicklineColor = OxyColors.White,
                    IsZoomEnabled = false,
                    IsPanEnabled = false
                });

                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Liczba występów",
                    Minimum = 0,
                    TitleColor = OxyColors.White,
                    TextColor = OxyColors.White,
                    TicklineColor = OxyColors.White,
                    IsZoomEnabled = false,
                    IsPanEnabled = false
                });
            }

            model.Series.Add(histogram);
            return model;
        }
    }
}
