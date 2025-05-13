using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Misc;
using KarateSystem.Models;
using KarateSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Service
{
    public class ReusltStatsService : IResultStatsService
    {
        private ApplicationDbContext _dbContext;
        public ReusltStatsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<string, int[]>> GetMedalStatsByClubAsync(int tourId)
        {
            var fights = await _dbContext.Fights
                .Include(f => f.TourCatKumite)
                .Where(f => f.TourCatKumite.TourId == tourId)
                .ToListAsync();

            var competitors = await _dbContext.TourCompetitors
                .Include(tc => tc.Competitor)
                    .ThenInclude(c => c.Club)
                .Where(tc => tc.TourId == tourId)
                .ToListAsync();

            var kataResults = await _dbContext.Katas
                .Include(k => k.TourCompetitor)
                    .ThenInclude(tc => tc.Competitor)
                        .ThenInclude(c => c.Club)
                .Where(k => k.TourCompetitor.TourId == tourId)
                .ToListAsync();

            var medalStats = new Dictionary<string, int[]>(); // Klub -> [Złoto, Srebro, Brąz]

            // Przetwarzanie kumite
            ProcessKumiteMedals(fights, competitors, medalStats);

            // Przetwarzanie kata
            ProcessKataMedals(kataResults, medalStats);

            return medalStats;
        }

        private void ProcessKumiteMedals(List<Fight> fights, List<TourCompetitor> competitors, Dictionary<string, int[]> medalStats)
        {
            var fightsByCategory = fights.GroupBy(f => f.TourCatKumiteId);

            foreach (var category in fightsByCategory)
            {
                var categoryFights = category.ToList();
                if (!categoryFights.Any()) continue;

                var rounds = categoryFights.Select(f => f.Round).Distinct().ToList();
                var maxRound = rounds.Max();
                var semiFinalRound = rounds
                    .Where(r => r < maxRound)
                    .OrderByDescending(r => r)
                    .FirstOrDefault();

                // FINAŁ
                var finalFight = categoryFights.FirstOrDefault(f => f.Round == maxRound);
                if (finalFight != null)
                {
                    var winner = competitors.FirstOrDefault(tc => tc.Competitor.CompId == finalFight.WinnerId)?.Competitor;
                    var loserId = finalFight.RedCompetitorId == finalFight.WinnerId ? finalFight.BlueCompetitorId : finalFight.RedCompetitorId;
                    var loser = competitors.FirstOrDefault(tc => tc.Competitor.CompId == loserId)?.Competitor;

                    AddMedal(medalStats, winner, 0); // Złoto
                    AddMedal(medalStats, loser, 1);  // Srebro
                }

                // PÓŁFINAŁY → przegrani dostają brąz
                var semiFinals = categoryFights
                    .Where(f => f.Round == semiFinalRound)
                    .ToList();

                foreach (var fight in semiFinals)
                {
                    if (fight.WinnerId == null) continue;

                    var loserId = fight.RedCompetitorId == fight.WinnerId ? fight.BlueCompetitorId : fight.RedCompetitorId;
                    var loser = competitors.FirstOrDefault(tc => tc.Competitor.CompId == loserId)?.Competitor;

                    AddMedal(medalStats, loser, 2); // Brąz
                }
            }
        }

        private void ProcessKataMedals(List<Kata> kataResults, Dictionary<string, int[]> medalStats)
        {
            var kataByCategory = kataResults.GroupBy(k => k.TourCompetitor.TourCatKataId);

            foreach (var category in kataByCategory)
            {
                var categoryResults = category.ToList();
                if (!categoryResults.Any()) continue;

                var sortedResults = categoryResults
                    .OrderByDescending(k => k.KataScore)
                    .ThenBy(k => k.Overtime)
                    .ToList();

                for (int i = 0; i < Math.Min(3, sortedResults.Count); i++)
                {
                    var competitor = sortedResults[i].TourCompetitor.Competitor;
                    AddMedal(medalStats, competitor, i); // 0-złoto, 1-srebro, 2-brąz
                }
            }
        }

        private void AddMedal(Dictionary<string, int[]> medalStats, Competitor comp, int medalIndex)
        {
            if (comp?.Club == null) return;

            var clubName = comp.Club.ClubName;
            if (!medalStats.ContainsKey(clubName))
                medalStats[clubName] = new int[3];

            medalStats[clubName][medalIndex]++;
        }

        public async Task<Dictionary<string, int>> GetClubParticipationAsync(int tourId)
        {
            return await _dbContext.TourCompetitors
                .Where(tc => tc.TourId == tourId)
                .GroupBy(tc => tc.Competitor.Club.ClubName)
                .Select(g => new { Club = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Club, x => x.Count);
        }

        public async Task<(int MaleCount, int FemaleCount)> GetGenderDistributionAsync(int tourId)
        {
            var males = await _dbContext.TourCompetitors
                .CountAsync(tc => tc.TourId == tourId && tc.Competitor.CompGender == true);
            var females = await _dbContext.TourCompetitors
                .CountAsync(tc => tc.TourId == tourId && tc.Competitor.CompGender == false);
            return (males, females);
        }

        public async Task<List<int>> GetCompetitorsAgesAsync(int tourId)
        {
            var currentYear = DateTime.Now.Year;
            return await _dbContext.TourCompetitors
                .Where(tc => tc.TourId == tourId)
                .Select(tc => Helper.CalculateAge(tc.Competitor.CompDateOfBirth))
                .ToListAsync();
        }
        public async Task<List<int>> GetFightDurationsAsync(int tourId)
        {
            return await _dbContext.Fights
                .Where(f => f.TourCatKumite.TourId == tourId && f.FightTime.HasValue)
                .Select(f => f.FightTime.Value)
                .ToListAsync();
        }
        public async Task<Dictionary<string, int>> GetFightsPerCategoryAsync(int tourId)
        {
            return await _dbContext.Fights
                .Where(f => f.TourCatKumite.TourId == tourId)
                .GroupBy(f => f.TourCatKumite.KumiteCategory != null ?
                             f.TourCatKumite.KumiteCategory.KumiteCatName : "Brak kategorii")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category, x => x.Count);
        }
        public async Task<Dictionary<string, int>> GetKatasPerCategoryAsync(int tourId)
        {
            return await _dbContext.TourCompetitors
                .Where(tc => tc.TourId == tourId)
                .GroupBy(tc => tc.TourCatKataId != null
                    ? tc.TourCatKata.KataCategory.KataCatName : "Nie bierze udziału w kata")
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category, x => x.Count);
        }
        public async Task<List<decimal>> GetScoresInKatasAsync(int tourId)
        {
            return await _dbContext.Katas
                .Include(k => k.TourCompetitor) 
                .Where(k => k.TourCompetitor.TourId == tourId && k.KataScore.HasValue)
                .Select(k => Math.Round(k.KataScore.Value, 2)) 
                .OrderBy(score => score) 
                .ToListAsync();
        }
    }
}
