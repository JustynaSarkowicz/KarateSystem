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

            services.AddScoped<ICompetitorRepository, CompetitorRepository>();
            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IMatRepository, MatRepository>();
            services.AddScoped<IDegreeRepository, DegreeRepository>();
            services.AddScoped<ICataCategoryRepository, CataCategoryRepository>();
            services.AddScoped<ICatKataDegreeRepository, CatKataDegreeRepository>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<CompetitorsViewModel>();
            services.AddScoped<ClubsDegreesMatsViewModel>();
            services.AddScoped<CategoryViewModel>();
            services.AddScoped<MainViewModel>();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainViewModel>() // Ustawienie ViewModel
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
