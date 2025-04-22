using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Misc;
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
    public class TournamentRepository : ITournamentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TournamentRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddTourAsync(TournamentDto tournament)
        {
            var existingTour = await _dbContext.Tournaments
                .AnyAsync(t => t.TourName == tournament.TourName &&
                          t.TourDate == tournament.TourDate &&
                          t.TourId != tournament.TourId);
            if (existingTour)
            {
                throw new Exception("Turniej o tej samej nazwie i dacie już istnieje.");
            }

            var newTour = _mapper.Map<Tournament>(tournament);

            _dbContext.Tournaments.Add(newTour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TournamentDto>> GetAllTournamentsAsync()
        {
            var entities = await _dbContext.Tournaments.AsNoTracking().ToListAsync();

            return _mapper.Map<List<TournamentDto>>(entities);
        }

        public async Task UpdateTournamentAsync(TournamentDto tournament)
        {
            var existingTour = await _dbContext.Tournaments
                .FirstOrDefaultAsync(t => t.TourId == tournament.TourId);
            
            if (existingTour == null)
            {
                throw new Exception("Nie znaleziono turnieju do edycji.");
            }

            var tourTaken = await _dbContext.Tournaments
                .AnyAsync(t => t.TourName == tournament.TourName &&
                          t.TourDate == tournament.TourDate &&
                          t.TourId != tournament.TourId);
            if (tourTaken)
            {
                throw new Exception("Turniej o tej samej nazwie i dacie już istnieje.");
            }

            existingTour.TourName = tournament.TourName;
            existingTour.TourPlace = tournament.TourPlace;
            existingTour.TourDate = tournament.TourDate;
            existingTour.Status = tournament.Status;

            _dbContext.Tournaments.Update(existingTour);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<TournamentDto> GetLastFinishedTournament()
        {
            var status = Helper.StatusOptionsList.Where(opt => opt.DisplayName == "Zakończony").Select(opt => opt.Value).FirstOrDefault();
            var lastTour = await _dbContext.Tournaments
                .Include(t => t.TourCatKatas)
                    .ThenInclude(t => t.KataCategory)
                .Include(t => t.TourCatKumites)
                    .ThenInclude(t => t.KumiteCategory)
                .Include(t => t.TourCompetitors)
                    .ThenInclude(t => t.Competitor)
                    .ThenInclude(t => t.Club)
                .Include(t => t.TourCompetitors)
                    .ThenInclude(t => t.Kata)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.TourDate)
                .FirstOrDefaultAsync();
            if(lastTour == null)
            {
                return null;
            }
            return _mapper.Map<TournamentDto>(lastTour);
        }
    }
}
