using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;

namespace KarateSystem.Repository
{
    public class CompetitorRepository : ICompetitorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public event EventHandler CompChanged;

        public CompetitorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public async Task AddCompAsync(CompetitorDto competitor)
        {
            var existingComp = await _dbContext.Competitors
                .AnyAsync(c => c.CompFirstName == competitor.CompFirstName &&
                                          c.CompLastName == competitor.CompLastName &&
                                          c.CompDateOfBirth == competitor.CompDateOfBirth &&
                                          c.CompId != competitor.CompId);
            if (existingComp)
            {
                throw new Exception("Zawodnik o takim imieniu i nazwisku oraz dacie urodzenia już istnieje.");
            }

            var newComp = _mapper.Map<Competitor>(competitor);

            _dbContext.Competitors.Add(newComp);
            await _dbContext.SaveChangesAsync();

            CompChanged?.Invoke(this, EventArgs.Empty);
        }
        public async Task<List<CompetitorDto>> GetAllCompetitorsAsync()
        {
            var entities = await _dbContext.Competitors
                .Include(k => k.Club)
                .Include(k => k.Degree)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<CompetitorDto>>(entities);
        }
        public async Task UpdateCompAsync(CompetitorDto competitor)
        {
            var existingComp = await _dbContext.Competitors
                .FirstOrDefaultAsync(c => c.CompId == competitor.CompId);

            if (existingComp == null)
                throw new Exception("Nie znaleziono zawodnika do edycji.");
            
            var compTaken =  await _dbContext.Competitors
                .AnyAsync(c => c.CompFirstName == competitor.CompFirstName &&
                                          c.CompLastName == competitor.CompLastName &&
                                          c.CompDateOfBirth == competitor.CompDateOfBirth &&
                                          c.CompId != competitor.CompId);
            if (compTaken)
                throw new Exception("Zawodnik o takim imieniu i nazwisku oraz dacie urodzenia już istnieje.");

            existingComp.CompFirstName = competitor.CompFirstName;
            existingComp.CompLastName = competitor.CompLastName;
            existingComp.CompDateOfBirth = competitor.CompDateOfBirth;
            existingComp.CompGender = competitor.CompGender;
            existingComp.CompWeight = competitor.CompWeight;
            existingComp.CompDegreeId = competitor.CompDegreeId;
            existingComp.CompClubId = competitor.CompClubId;
            
            _dbContext.Competitors.Update(existingComp);
            await _dbContext.SaveChangesAsync();
            CompChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
