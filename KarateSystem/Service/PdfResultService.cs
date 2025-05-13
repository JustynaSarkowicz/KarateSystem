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

        public async Task<List<KumiteResultDto>> GetKumiteResultsAsync(int tourId)
        {
            var fights = await _dbContext.Fights
                .Where(f => f.RedCompetitor.TourId == tourId || f.BlueCompetitor.TourId == tourId)
                .Include(f => f.RedCompetitor).ThenInclude(tc => tc.Competitor).ThenInclude(c => c.Club)
                .Include(f => f.BlueCompetitor).ThenInclude(tc => tc.Competitor).ThenInclude(c => c.Club)
                .Include(f => f.TourCatKumite).ThenInclude(tc => tc.KumiteCategory)
                .ToListAsync();

            var results = new List<KumiteResultDto>();

            var fightsByCategory = fights.GroupBy(f => f.TourCatKumiteId);

            foreach (var categoryGroup in fightsByCategory)
            {
                var categoryFights = categoryGroup.ToList();
                if (!categoryFights.Any()) continue;

                var categoryName = categoryFights.First().TourCatKumite.KumiteCategory.KumiteCatName;

                var rounds = categoryFights.Select(f => f.Round).Distinct().ToList();
                var maxRound = rounds.Max();
                var semiFinalRound = rounds.Where(r => r < maxRound).OrderByDescending(r => r).FirstOrDefault();

                // FINAŁ
                var finalFight = categoryFights.FirstOrDefault(f => f.Round == maxRound);
                if (finalFight != null)
                {
                    var winner = finalFight.WinnerId == finalFight.RedCompetitorId
                        ? finalFight.RedCompetitor.Competitor
                        : finalFight.BlueCompetitor.Competitor;

                    var loser = finalFight.WinnerId == finalFight.RedCompetitorId
                        ? finalFight.BlueCompetitor.Competitor
                        : finalFight.RedCompetitor.Competitor;

                    results.Add(new KumiteResultDto
                    {
                        Place = 1,
                        FullName = $"{winner.CompFirstName} {winner.CompLastName}",
                        ClubName = winner.Club?.ClubName ?? "brak",
                        CategoryName = categoryName
                    });

                    results.Add(new KumiteResultDto
                    {
                        Place = 2,
                        FullName = $"{loser.CompFirstName} {loser.CompLastName}",
                        ClubName = loser.Club?.ClubName ?? "brak",
                        CategoryName = categoryName
                    });
                }

                // PÓŁFINAŁY → przegrani = dwa brązy
                var semiFinals = categoryFights
                        .Where(f => f.Round == semiFinalRound && f.WinnerId != null)
                        .ToList();

                foreach (var fight in semiFinals)
                {
                    if (fight.FightWalkover == true)
                        continue; // pomiń walki z walkowerem

                    var redComp = fight.RedCompetitor?.Competitor;
                    var blueComp = fight.BlueCompetitor?.Competitor;

                    if (redComp == null || blueComp == null)
                        continue; // pomiń, jeśli któryś zawodnik nie istnieje

                    var loser = fight.RedCompetitorId == fight.WinnerId ? blueComp : redComp;

                    results.Add(new KumiteResultDto
                    {
                        Place = 3,
                        FullName = $"{loser.CompFirstName} {loser.CompLastName}",
                        ClubName = loser.Club?.ClubName ?? "brak",
                        CategoryName = categoryName
                    });
                }

            }

            return results
                .OrderBy(r => r.CategoryName)
                .ThenBy(r => r.Place)
                .ToList();
        }

    }
}
