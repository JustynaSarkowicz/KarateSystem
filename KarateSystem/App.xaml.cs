using KarateSystem.Configurations;
using KarateSystem.JsonManager;
using KarateSystem.MappingProfiles;
using KarateSystem.Repository;
using KarateSystem.Repository.Interfaces;
using KarateSystem.Service;
using KarateSystem.Service.Interfaces;
using KarateSystem.ViewModel;
using KarateSystem.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using AutoMapper;
using System.Windows;
using System.Windows.Input;

namespace KarateSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseSqlServer(JsonConfiguration.GetSqlConnectionString()),
                     ServiceLifetime.Transient);

            services.AddTransient<ICompetitorRepository, CompetitorRepository>();
            services.AddTransient<IClubRepository, ClubRepository>();
            services.AddTransient<IMatRepository, MatRepository>();
            services.AddTransient<IDegreeRepository, DegreeRepository>();
            services.AddTransient<IKataCategoryRepository, KataCategoryRepository>();
            services.AddTransient<ICatKataDegreeRepository, CatKataDegreeRepository>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IKumiteCategoryRepository, KumiteCategoryRepository>();
            services.AddTransient<ITourCompetitorRepository, TourCompetitorRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITournamentRepository, TournamentRepository>();
            services.AddTransient<ITourCatKataRepository, TourCatKataRepository>();
            services.AddTransient<ITourCatKumiteRepository, TourCatKumiteRepository>();
            services.AddTransient<IKataRepository, KataRepository>();
            services.AddTransient<IFightRepository, FightRepository>();
            services.AddTransient<IResultStatsService, ReusltStatsService>();
            services.AddTransient<IPdfResultService, PdfResultService>();
            services.AddTransient<CompetitorsViewModel>();
            services.AddTransient<ClubsDegreesMatsViewModel>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<TournamentViewModel>();
            services.AddTransient<AddCompetitorsViewModel>();
            services.AddTransient<SetCompToCatTourViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<KataKumiteViewModel>();
            services.AddTransient<ResultAnalysisViewModel>();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddTransient<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainViewModel>() 
            });
            services.AddTransient<AddCompetitorsView>(provider => new AddCompetitorsView()
            {
                DataContext = provider.GetRequiredService<AddCompetitorsViewModel>() 
            });
            services.AddTransient<SetCompToCatTourView>(provider => new SetCompToCatTourView()
            {
                DataContext = provider.GetRequiredService<SetCompToCatTourViewModel>() 
            });
            services.AddTransient<LoginView>(provider => new LoginView()
            {
                DataContext = provider.GetRequiredService<LoginViewModel>() 
            });
            services.AddTransient<LoginViewModel>(provider =>
                                new LoginViewModel(
                                    provider.GetRequiredService<IUserRepository>(),
                                    provider));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginView = serviceProvider.GetRequiredService<LoginView>();
            loginView.Show();
        }
    }
}
