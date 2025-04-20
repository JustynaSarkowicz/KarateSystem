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
    public class TourCatKumiteRepository : ITourCatKumiteRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TourCatKumiteRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<TourCatKumiteDto>> GetCatKumiteByIdTourAsync(int tourId)
        {
            var tourCatKumite = await _dbContext.TourCatKumites.Where(t => t.TourId == tourId)
                .Include(t => t.KumiteCategory)
                .Include(t => t.Mat)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TourCatKumiteDto>>(tourCatKumite);
        }
        public async Task DeleteCatKumiteFromTour(int tourCatKumiteId)
        {
            var tourCatKumite = await _dbContext.TourCatKumites.FindAsync(tourCatKumiteId);
            if (tourCatKumite == null)
                throw new Exception("Nie znaleziono kategorii kumite w turnieju do usunięcia.");

            _dbContext.TourCatKumites.Remove(tourCatKumite);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddCatKumiteToTour(TourCatKumiteDto tourCatKumiteDto)
        {
            var existingTourCatKumite = await _dbContext.TourCatKumites
                .AnyAsync(t => t.TourId == tourCatKumiteDto.TourId &&
                               t.KumiteCatId == tourCatKumiteDto.KumiteCatId);
            if (existingTourCatKumite)
            {
                throw new Exception("Ta kategoria kumite już istnieje w tym turnieju.");
            }
            var newTourCatKumite = _mapper.Map<TourCatKumite>(tourCatKumiteDto);
            _dbContext.TourCatKumites.Add(newTourCatKumite);
            await _dbContext.SaveChangesAsync();
        }
    }
}
