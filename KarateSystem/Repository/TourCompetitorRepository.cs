using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Repository
{
    public class TourCompetitorRepository : ITourCompetitorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TourCompetitorRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<TourCompetitorDto>> GetTourCompetitorsByIdTourAsync(int tourId)
        {
            var tourCompetitors = await _dbContext.TourCompetitors.Where(t => t.TourId == tourId)
                .Include(t => t.Competitor)
                    .ThenInclude(c => c.Club)
                .Include(t => t.Competitor)
                    .ThenInclude(d => d.Degree)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TourCompetitorDto>>(tourCompetitors);
        }
        public async Task<List<TourCompetitorDto>> GetCompetitorToursByIdCompAsync(int compId)
        {
            var tourCompetitors = await _dbContext.TourCompetitors.Where(t => t.CompId == compId)
                .Include(t => t.Tournament)
                .Include(t => t.TourCatKata)
                    .ThenInclude(t => t.KataCategory)
                .Include(t => t.TourCatKumite)
                    .ThenInclude(t => t.KumiteCategory)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TourCompetitorDto>>(tourCompetitors);
        }
        public async Task<List<TourCompetitorDto>> GetCompetitorToursByCatKataIdAsync(int catKataId)
        {
            var tourCompetitors = await _dbContext.TourCompetitors.Where(t => t.TourCatKataId == catKataId)
                .Include(t => t.Competitor)
                    .ThenInclude(c => c.Club)
                .Include(t => t.Competitor)
                    .ThenInclude(d => d.Degree)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<TourCompetitorDto>>(tourCompetitors);
        }
        public async Task<List<TourCompetitorDto>> GetCompetitorToursByCatKumiteIdAsync(int catKumiteId)
        {
            var tourCompetitors = await _dbContext.TourCompetitors.Where(t => t.TourCatKumiteId == catKumiteId)
                .Include(t => t.Competitor)
                    .ThenInclude(c => c.Club)
                .Include(t => t.Competitor)
                    .ThenInclude(d => d.Degree)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<TourCompetitorDto>>(tourCompetitors);
        }
        public async Task DeleteTourComp(int tourCompId)
        {
            var tourComp = await _dbContext.TourCompetitors.FindAsync(tourCompId);
            if (tourComp == null)
                throw new Exception("Nie znaleziono zawodnika w turnieju do usunięcia.");

            _dbContext.TourCompetitors.Remove(tourComp);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddCompToTour(TourCompetitorDto tourCompetitor)
        {
            var tourComp = _mapper.Map<TourCompetitor>(tourCompetitor);
            _dbContext.TourCompetitors.Add(tourComp);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddCompToTourCatKata(TourCompetitorDto tourCompetitor, int kataCatId)
        {
            var tourComp = await _dbContext.TourCompetitors
                .FirstOrDefaultAsync(tc => tc.CompId == tourCompetitor.CompId && tc.TourId == tourCompetitor.TourId && tc.TourCatKataId == null);

            if (tourComp == null)
                throw new Exception("Zawodnik już jest przypisany do innej kategorii kata lub nie istnieje.");

            tourComp.TourCatKataId = kataCatId;

            _dbContext.TourCompetitors.Update(tourComp);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddCompToTourCatKumite(TourCompetitorDto tourCompetitor, int kumiteCatId)
        {
            var tourComp = await _dbContext.TourCompetitors
                .FirstOrDefaultAsync(tc => tc.CompId == tourCompetitor.CompId && tc.TourId == tourCompetitor.TourId && tc.TourCatKumiteId == null);

            if (tourComp == null)
                throw new Exception("Zawodnik już jest przypisany do innej kategorii kata lub nie istnieje.");

            tourComp.TourCatKumiteId = kumiteCatId;

            _dbContext.TourCompetitors.Update(tourComp);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteCompFromTourCatKata(TourCompetitorDto tourCompetitor)
        {
            var tourComp = await _dbContext.TourCompetitors
                .FirstOrDefaultAsync(tc => tc.CompId == tourCompetitor.CompId && tc.TourId == tourCompetitor.TourId && tc.TourCatKataId != null);
            if (tourComp == null)
                throw new Exception("Nie można znaleźć zawodnika.");

            tourComp.TourCatKataId = null;
            _dbContext.TourCompetitors.Update(tourComp);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteCompFromTourCatKumite(TourCompetitorDto tourCompetitor)
        {
            var tourComp = await _dbContext.TourCompetitors
                .FirstOrDefaultAsync(tc => tc.CompId == tourCompetitor.CompId && tc.TourId == tourCompetitor.TourId && tc.TourCatKumiteId != null);
            if (tourComp == null)
                throw new Exception("Nie można znaleźć zawodnika.");

            tourComp.TourCatKumiteId = null;
            _dbContext.TourCompetitors.Update(tourComp);
            await _dbContext.SaveChangesAsync();
        }
    }
}
