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
    public class KataRepository : IKataRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public KataRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<KataDto>> GetKatasByTourCatKataIdAsync(int tourCatKataId)
        {
            var katas =  await _dbContext.Katas
                .Include(k => k.TourCompetitor)
                .ThenInclude(k => k.Competitor)
                .Where(tc => tc.TourCompetitor.TourCatKataId == tourCatKataId)
                .AsNoTracking()
                .ToListAsync();
            var kataDtos = _mapper.Map<List<KataDto>>(katas);

            for (int i = 0; i < kataDtos.Count; i++)
            {
                kataDtos[i].Numeration = i + 1;
            }

            return kataDtos;
        }

        public async Task UpdateGradesOnKataAsync(KataDto kata)
        {
            var existingKata = await _dbContext.Katas
                .FirstOrDefaultAsync(k => k.KataId == kata.KataId);
            if (existingKata == null)
                throw new Exception("Nie znaleziono kata do edycji.");

            existingKata.KataRate1 = kata.KataRate1;
            existingKata.KataRate2 = kata.KataRate2;
            existingKata.KataRate3 = kata.KataRate3;
            existingKata.KataRate4 = kata.KataRate4;
            existingKata.KataRate5 = kata.KataRate5;
            existingKata.Overtime = kata.Overtime;

            var rates = new List<decimal?>
            {
                existingKata.KataRate1,
                existingKata.KataRate2,
                existingKata.KataRate3,
                existingKata.KataRate4,
                existingKata.KataRate5
            };

            rates.Sort();
            rates.RemoveAt(0); 
            rates.RemoveAt(rates.Count - 1);

            existingKata.KataScore = rates.Count == 3 ? rates.Average() : null;

            await _dbContext.SaveChangesAsync();
        }
    }
}
