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
    }
}
