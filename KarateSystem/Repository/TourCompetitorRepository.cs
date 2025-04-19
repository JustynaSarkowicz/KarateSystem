using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Repository
{
    public class TourCompetitorRepository : ITourCompetitorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TourCompetitorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TourCompetitorDto>> GetTourCompetitorsByIdTourAsync(int tourId)
        {
            var tourCompetitors = await _context.TourCompetitors.Where(t => t.TourId == tourId)
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
            var tourCompetitors = await _context.TourCompetitors.Where(t => t.CompId == compId)
                .Include(t => t.Tournament)
                .Include(t => t.TourCatKata)
                    .ThenInclude(t => t.KataCategory)
                .Include(t => t.TourCatKumite)
                    .ThenInclude(t => t.KumiteCategory)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TourCompetitorDto>>(tourCompetitors);
        }
    }
}
