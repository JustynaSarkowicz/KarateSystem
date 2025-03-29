using KarateSystem.Configurations;
using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Configurations
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        //public DbSet<Fight> Fights { get; set; }
        public DbSet<Kata> Katas{ get; set; }
        public DbSet<KataCategory> KataCategories { get; set; }
        public DbSet<KumiteCategory> KumiteCategories { get; set; }
        public DbSet<Mat> Mats { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TourCompetitor> TourCompetitors { get; set; }
        public DbSet<TourCatKata> TourCatKatas { get; set; }
        public DbSet<TourCatKumite> TourCatKumites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new CompetitorConfiguration());
            modelBuilder.ApplyConfiguration(new DegreeConfiguration());
            //modelBuilder.ApplyConfiguration(new FightConfiguration());
            modelBuilder.ApplyConfiguration(new KataConfiguration());
            modelBuilder.ApplyConfiguration(new KataCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new KumiteCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MatConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentConfiguration());
            modelBuilder.ApplyConfiguration(new TourCompetitorConfiguration());
            modelBuilder.ApplyConfiguration(new TourCatKataConfiguration());
            modelBuilder.ApplyConfiguration(new TourCatKumiteConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Database.EnsureCreated() sprawdza, czy baza danych istnieje.
            // Jeśli tak - nic nie robi. Jeśli nie - tworzy bazę i tabele zgodnie z modelem.
            // UWAGA: Gdy baza istnieje, nie jest sprawdzane, czy jest zgodna z modelem.
            Database.EnsureCreated();
        }
    }

}
