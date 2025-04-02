using KarateSystem.Configurations;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Repository
{
    public class CompetitorRepository : ICompetitorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CompetitorRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public List<Competitor> GetAllCompetitors()
        {
            var competitors = _dbContext.Competitors
                .Include(c => c.Degree)
                .Include(c => c.Club)
                .ToList();
            return competitors;
        }
        public Competitor? GetCompetitor(int compId)
        {
            var competitor = _dbContext.Competitors.Where(c => c.CompId == compId).FirstOrDefault();
            if (competitor == null) return null;
            return competitor;
        }
        public List<Tournament> GetCompetiorTournament(int compId)
        {
            var tournaments = _dbContext.TourCompetitors
                                        .Where(tc => tc.CompId == compId)
                                        .Select(tc => tc.Tournament)
                                        .ToList();
            return tournaments;
        }
        public string GetCompeitorTourCatKata(int compId, int tourId)
        {
            var kataCategory = _dbContext.TourCompetitors
                                        .Where(tc => tc.CompId == compId && tc.TourId == tourId)
                                        .Select(tc => tc.TourCatKata.KataCategory.KataCatName).ToString();
            if (string.IsNullOrEmpty(kataCategory)) return "";
            return kataCategory;
        }
        public string GetCompeitorTourCatKumite(int compId, int tourId)
        {
            var kumiteCategory = _dbContext.TourCompetitors
                                        .Where(tc => tc.CompId == compId && tc.TourId == tourId)
                                        .Select(tc => tc.TourCatKumite.KumiteCategory.KumiteCatName).ToString();
            if (string.IsNullOrEmpty(kumiteCategory)) return "";
            return kumiteCategory;
        }
    }
}
