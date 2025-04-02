using KarateSystem.JsonManager;
using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Configurations
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Kata> Katas { get; set; }
        public DbSet<KataCategory> KataCategories { get; set; }
        public DbSet<KumiteCategory> KumiteCategories { get; set; }
        public DbSet<Mat> Mats { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TourCompetitor> TourCompetitors { get; set; }
        public DbSet<TourCatKata> TourCatKatas { get; set; }
        public DbSet<TourCatKumite> TourCatKumites { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new CompetitorConfiguration());
            modelBuilder.ApplyConfiguration(new DegreeConfiguration());
            modelBuilder.ApplyConfiguration(new FightConfiguration());
            modelBuilder.ApplyConfiguration(new KataConfiguration());
            modelBuilder.ApplyConfiguration(new KataCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new KumiteCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MatConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentConfiguration());
            modelBuilder.ApplyConfiguration(new TourCompetitorConfiguration());
            modelBuilder.ApplyConfiguration(new TourCatKataConfiguration());
            modelBuilder.ApplyConfiguration(new TourCatKumiteConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(JsonConfiguration.GetSqlConnectionString());
        }
    }
}