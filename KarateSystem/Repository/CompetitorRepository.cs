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
        public async Task<List<Competitor>> GetAllCompetitorsAsync()
        {
            var competitors = await _dbContext.Competitors
                .Include(c => c.Degree)
                .Include(c => c.Club)
                .ToListAsync();  
            return competitors;
        }
        public async Task<Competitor?> GetCompetitorAsync(int compId)
        {
            var competitor = await _dbContext.Competitors
                .Where(c => c.CompId == compId)
                .FirstOrDefaultAsync();  
            return competitor;
        }
        public async Task<List<Tournament>> GetCompetitorTournamentAsync(int compId)
        {
            var tournaments = await _dbContext.TourCompetitors
                .Where(tc => tc.CompId == compId)
                .Select(tc => tc.Tournament)
                .ToListAsync();  
            return tournaments;
        }
        public async Task<string> GetCompetitorTourCatKataAsync(int compId, int tourId)
        {
            var kataCategory = await _dbContext.TourCompetitors
                .Where(tc => tc.CompId == compId && tc.TourId == tourId)
                .Select(tc => tc.TourCatKata.KataCategory.KataCatName)
                .FirstOrDefaultAsync();  

            if (string.IsNullOrEmpty(kataCategory)) return "";
            return kataCategory;
        }
        public async Task<string> GetCompetitorTourCatKumiteAsync(int compId, int tourId)
        {
            var kumiteCategory = await _dbContext.TourCompetitors
                .Where(tc => tc.CompId == compId && tc.TourId == tourId)
                .Select(tc => tc.TourCatKumite.KumiteCategory.KumiteCatName)
                .FirstOrDefaultAsync(); 

            if (string.IsNullOrEmpty(kumiteCategory)) return "";
            return kumiteCategory;
        }
    }

}
