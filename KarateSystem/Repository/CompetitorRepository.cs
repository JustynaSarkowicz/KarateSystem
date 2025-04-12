using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Repository
{
    public class CompetitorRepository : ICompetitorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CompetitorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public async Task<bool> AddCompAsync(CompetitorDto competitor)
        {
            if (string.IsNullOrWhiteSpace(competitor.CompFirstName) ||
                string.IsNullOrWhiteSpace(competitor.CompLastName) ||
                competitor.CompWeight < 0 ||
                competitor.CompDegreeId <= 0 ||
                competitor.CompClubId <= 0)
            {
                return false;
            }
            var existingComp = await _dbContext.Competitors
                .FirstOrDefaultAsync(c => c.CompFirstName == competitor.CompFirstName &&
                                          c.CompLastName == competitor.CompLastName &&
                                          c.CompDateOfBirth == competitor.CompDateOfBirth);
            if (existingComp != null) return false;
            var newComp = _mapper.Map<Competitor>(competitor);
            try
            {
                _dbContext.Competitors.Add(newComp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
        }
        public async Task<List<CompetitorDto>> GetAllCompetitorsAsync()
        {
            var entities = await _dbContext.Competitors
                .Include(k => k.Club)
                .Include(k => k.Degree)
                .Include(k => k.TourCompetitors)
                    .ThenInclude(tc => tc.Tournament)
                .Include(c => c.TourCompetitors)
                    .ThenInclude(tc => tc.TourCatKata)
                    .ThenInclude(tck => tck.KataCategory)
                .Include(c => c.TourCompetitors)
                    .ThenInclude(tc => tc.TourCatKumite)
                    .ThenInclude(tck => tck.KumiteCategory)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<CompetitorDto>>(entities);
        }
        public async Task<bool> UpdateCompAsync(CompetitorDto competitor)
        {
            var existingComp = await _dbContext.Competitors
                .FirstOrDefaultAsync(c => c.CompId == competitor.CompId);
            if (existingComp == null) return false;
            if (string.IsNullOrWhiteSpace(competitor.CompFirstName) ||
                string.IsNullOrWhiteSpace(competitor.CompLastName) ||
                competitor.CompWeight < 0 ||
                competitor.CompDegreeId <= 0 ||
                competitor.CompClubId <= 0)
            {
                return false;
            }

            existingComp.CompFirstName = competitor.CompFirstName;
            existingComp.CompLastName = competitor.CompLastName;
            existingComp.CompDateOfBirth = competitor.CompDateOfBirth;
            existingComp.CompGender = competitor.CompGender;
            existingComp.CompWeight = competitor.CompWeight;
            existingComp.CompDegreeId = competitor.CompDegreeId;
            existingComp.CompClubId = competitor.CompClubId;

            try
            {
                _dbContext.Competitors.Update(existingComp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
