using KarateSystem.JsonManager;
using KarateSystem.Models;
using KarateSystem.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Configurations
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Competitor> Competitors { get; set; }
        public virtual DbSet<Degree> Degrees { get; set; }
        public virtual DbSet<Fight> Fights { get; set; }
        public virtual DbSet<Kata> Katas { get; set; }
        public virtual DbSet<KataCategory> KataCategories { get; set; }
        public virtual DbSet<KumiteCategory> KumiteCategories { get; set; }
        public virtual DbSet<Mat> Mats { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<TourCompetitor> TourCompetitors { get; set; }
        public virtual DbSet<TourCatKata> TourCatKatas { get; set; }
        public virtual DbSet<TourCatKumite> TourCatKumites { get; set; }
        public virtual DbSet<CatKataDegree> CatKataDegrees { get; set; }
        public ApplicationDbContext()
        {
        }
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
            modelBuilder.ApplyConfiguration(new CatKataDegreeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(JsonConfiguration.GetSqlConnectionString());
            }
        }
    }
}