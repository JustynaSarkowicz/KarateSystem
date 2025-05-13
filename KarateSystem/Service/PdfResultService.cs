using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Service
{
    public class PdfResultService : IPdfResultService
    {
        private readonly ApplicationDbContext _dbContext;
        public PdfResultService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<KataResultDto>> GetKataResultsAsync(int tourId)
        {
            var kataResults = await _dbContext.Katas
                .Where(k => k.TourCompetitor.TourId == tourId && k.TourCompetitor.TourCatKataId != null)
                .Include(k => k.TourCompetitor)
                    .ThenInclude(tc => tc.Competitor)
                        .ThenInclude(c => c.Club)
                .Include(k => k.TourCompetitor)
                    .ThenInclude(tc => tc.TourCatKata)
                        .ThenInclude(tck => tck.KataCategory)
                .Select(k => new KataResultDto
                {
                    FullName = k.TourCompetitor.Competitor.CompFirstName + " " + k.TourCompetitor.Competitor.CompLastName,
                    ClubName = k.TourCompetitor.Competitor.Club.ClubName,
                    Score = k.KataScore,
                    Overtime = k.Overtime,
                    CategoryName = k.TourCompetitor.TourCatKata.KataCategory.KataCatName
                })
                .ToListAsync();

            // Grupa i przypisanie miejsc wewnątrz każdej kategorii
            var resultWithPlaces = kataResults
                .GroupBy(r => r.CategoryName)
                .SelectMany(g =>
                {
                    var ordered = g.OrderByDescending(x => x.Score).ThenBy(x => x.Overtime).ToList();
                    for (int i = 0; i < ordered.Count; i++)
                    {
                        ordered[i].Place = i + 1;
                    }
                    return ordered;
                })
                .ToList();

            return resultWithPlaces;
        }


    }
}
