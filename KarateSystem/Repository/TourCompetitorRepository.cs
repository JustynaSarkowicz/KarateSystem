using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Misc;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
        public async Task<TourCompetitorDto> GetTourCompetitorByIdAsync(int tourCompId)
        {
            var tourCompetitor = await _dbContext.TourCompetitors.Where(t => t.TourCompId == tourCompId)
                .Include(t => t.Competitor)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return _mapper.Map<TourCompetitorDto>(tourCompetitor);
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

            var kata = new Kata
            {
                TourCompId = tourComp.TourCompId, 
                KataRate1 = null, 
                KataRate2 = null,
                KataRate3 = null,
                KataRate4 = null,
                KataRate5 = null,
                KataScore = null,
                Overtime = 0
            };

            _dbContext.Katas.Add(kata);
            tourComp.Kata = kata;

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
                .Include(tc => tc.Kata)
                .FirstOrDefaultAsync(tc => tc.CompId == tourCompetitor.CompId && tc.TourId == tourCompetitor.TourId && tc.TourCatKataId != null);
            if (tourComp == null)
                throw new Exception("Nie można znaleźć zawodnika.");

            if (tourComp.Kata != null)
            {
                _dbContext.Katas.Remove(tourComp.Kata);
            }

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

        public async Task<string> SetCompToCatKataAutomatic(int tourId)
        {
            int i = 0;
            var competitors = await _dbContext.TourCompetitors
                .Include(tc => tc.Competitor)
                .Where(tc => tc.TourId == tourId && tc.TourCatKataId == null)
                .ToListAsync();

            var kataCategories = await _dbContext.TourCatKatas
                .Include(tck => tck.KataCategory)
                    .ThenInclude(kc => kc.CatKataDegrees)
                .Where(tck => tck.TourId == tourId)
                .ToListAsync();

            foreach (var comp in competitors)
            {
                var age = Helper.CalculateAge(comp.Competitor.CompDateOfBirth);
                var gender = comp.Competitor.CompGender;
                var degreeId = comp.Competitor.CompDegreeId;

                var matchedCategory = kataCategories.FirstOrDefault(cat =>
                    age >= cat.KataCategory.KataCatAgeMin &&
                    age <= cat.KataCategory.KataCatAgeMax &&
                    (cat.KataCategory.KataCatGender == null || cat.KataCategory.KataCatGender == gender) &&
                    cat.KataCategory.CatKataDegrees.Any(d => d.DegreeId == degreeId));

                if (matchedCategory != null)
                {
                    comp.TourCatKataId = matchedCategory.TourCatKataId;
                    i++;
                }

                var kata = new Kata
                {
                    TourCompId = comp.TourCompId,
                    KataRate1 = null,
                    KataRate2 = null,
                    KataRate3 = null,
                    KataRate4 = null,
                    KataRate5 = null,
                    KataScore = null,
                    Overtime = 0
                };

                _dbContext.Katas.Add(kata);
                comp.Kata = kata;
            }

            await _dbContext.SaveChangesAsync();
            return $"Udało się dopasować {i}/{competitors.Count()} zawodników do kategorii kata.";
        }
        public async Task<string> SetCompToKumiteCatAutomatic(int tourId)
        {
            int i = 0;
            var competitors = await _dbContext.TourCompetitors
                .Include(tc => tc.Competitor)
                .Where(tc => tc.TourId == tourId && tc.TourCatKumiteId == null)
                .ToListAsync();

            var kumiteCategories = await _dbContext.TourCatKumites
                .Include(tck => tck.KumiteCategory)
                .Where(tck => tck.TourId == tourId)
                .ToListAsync();

            foreach (var comp in competitors)
            {
                var age = Helper.CalculateAge(comp.Competitor.CompDateOfBirth);
                var gender = comp.Competitor.CompGender;
                var weight = comp.Competitor.CompWeight;

                var matchedCategory = kumiteCategories.FirstOrDefault(cat =>
                    age >= cat.KumiteCategory.KumiteCatAgeMin &&
                    age <= cat.KumiteCategory.KumiteCatAgeMax &&
                    gender == cat.KumiteCategory.KumiteCatGender &&
                    weight >= cat.KumiteCategory.KumiteCatWeightMin &&
                    weight <= cat.KumiteCategory.KumiteCatWeightMax);

                if (matchedCategory != null)
                {
                    comp.TourCatKumiteId = matchedCategory.TourCatKumiteId;
                    i++;
                }
            }

            await _dbContext.SaveChangesAsync();
            return $"Udało się dopasować {i}/{competitors.Count()} zawodników do kategorii kumite.";
        }
    }
}
