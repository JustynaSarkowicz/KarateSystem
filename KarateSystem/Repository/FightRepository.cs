using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository
{
    public class FightRepository : IFightRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public FightRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> SetFightsAsync(int tournamentId)
        {
            bool result = false;
            try
            {
                var categories = await _dbContext.TourCatKumites
                    .Where(k => k.TourId == tournamentId)
                    .ToListAsync();

                foreach (var category in categories)
                {
                    // Pobierz zawodników w tej kategorii
                    var competitors = await _dbContext.TourCompetitors
                        .Where(tc => tc.TourCatKumiteId == category.TourCatKumiteId)
                        .OrderBy(_ => Guid.NewGuid()) // Losowe ustawienie zawodników
                        .ToListAsync();

                    if (competitors.Count < 2)
                        continue; // nie twórz walk, jeśli za mało zawodników

                    int round = 1;
                    int fightNumber = 1;
                    List<Fight> fights = new List<Fight>();

                    for (int i = 0; i < competitors.Count; i += 2)
                    {
                        var fight = new Fight
                        {
                            TourCatKumiteId = category.TourCatKumiteId,
                            FightNumOverTime = null,
                            Round = round,
                            FightWalkover = false,
                            RedCompetitorId = competitors[i].TourCompId,
                            BlueCompetitorId = (i + 1 < competitors.Count) ? competitors[i + 1].TourCompId : null,
                            WinnerId = null,
                            FightTime = null,
                            RedCompetitorScore = null,
                            BlueCompetitorScore = null
                        };

                        // Jeśli jest nieparzysta liczba zawodników - walkower
                        if (fight.BlueCompetitorId == null)
                        {
                            fight.FightWalkover = true;
                            fight.WinnerId = fight.RedCompetitorId;
                        }

                        fights.Add(fight);
                    }

                    _dbContext.Fights.AddRange(fights);
                    await _dbContext.SaveChangesAsync();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                Console.WriteLine($"Error: {ex.Message}");
                result = false;
            }
            return result;
        }

        public async Task<List<FightDto>> GetFightsByTourAsync(int tourCatKumiteId)
        {
            var fights = await _dbContext.Fights
                .Include(f => f.RedCompetitor).ThenInclude(rc => rc.Competitor)
                .Include(f => f.BlueCompetitor).ThenInclude(bc => bc.Competitor)
                .Include(f => f.Winner).ThenInclude(w => w.Competitor)
                .Where(f => f.TourCatKumite.TourCatKumiteId == tourCatKumiteId)
                .ToListAsync();

            var fightDtos = _mapper.Map<List<FightDto>>(fights);

            // Numerowanie np. wg kolejności walk w liście
            for (int i = 0; i < fightDtos.Count; i++)
            {
                fightDtos[i].FightNumber = i + 1;
            }

            return fightDtos;
        }

        public async Task UpdateFightsAsync(FightDto fightDto)
        {
            var fight = await _dbContext.Fights
                .FirstOrDefaultAsync(f => f.FightId == fightDto.FightId);

            if (fight == null)
                throw new Exception("Nie znaleziono walki o podanym ID.");

            // Aktualizacja danych
            fight.RedCompetitorScore = fightDto.RedCompetitorScore;
            fight.BlueCompetitorScore = fightDto.BlueCompetitorScore;
            fight.FightWalkover = fightDto.FightWalkover;
            fight.WinnerId = fightDto.WinnerId;
            fight.FightNumOverTime = fightDto.FightNumOverTime;
            fight.FightTime = fightDto.FightTime;

            await _dbContext.SaveChangesAsync();
        }

    }
}
