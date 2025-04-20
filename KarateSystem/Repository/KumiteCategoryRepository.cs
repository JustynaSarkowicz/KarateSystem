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
    public class KumiteCategoryRepository : IKumiteCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public event EventHandler KumiteCatChanged;
        public KumiteCategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task AddKumiteCategoryAsync(KumiteCategoryDto kumiteCategoryDto)
        {
            var existingKumiteCat = await _dbContext.KumiteCategories
                .AnyAsync(c => c.KumiteCatId != kumiteCategoryDto.KumiteCatId && c.KumiteCatName == kumiteCategoryDto.KumiteCatName);
            if (existingKumiteCat)
            {
                throw new Exception("Kategoria Kumite o takiej nazwie już istnieje.");
            }

            var kumiteCategory = _mapper.Map<KumiteCategory>(kumiteCategoryDto);
            
            _dbContext.KumiteCategories.Add(kumiteCategory);
            await _dbContext.SaveChangesAsync();

            KumiteCatChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<List<KumiteCategoryDto>> GetAllKumiteCategoryAsync()
        {
            var entities = await _dbContext.KumiteCategories.AsNoTracking().ToListAsync();
            return _mapper.Map<List<KumiteCategoryDto>>(entities);
        }

        public async Task UpdateKumiteCategoryAsync(KumiteCategoryDto kumiteCategoryDto)
        {
            var existingKumiteCategory = await _dbContext.KumiteCategories
                .FirstOrDefaultAsync(c => c.KumiteCatId == kumiteCategoryDto.KumiteCatId);

            if(existingKumiteCategory == null)
                throw new Exception("Nie znaleziono kategorii kumite do edycji.");
            
            var catKumiteTaken = await _dbContext.KumiteCategories
                .AnyAsync(c => c.KumiteCatName == kumiteCategoryDto.KumiteCatName && c.KumiteCatId != kumiteCategoryDto.KumiteCatId);

            if (catKumiteTaken)
                throw new Exception("Kategoria Kumite o takiej nazwie już istnieje.");

            existingKumiteCategory.KumiteCatName = kumiteCategoryDto.KumiteCatName;
            existingKumiteCategory.KumiteCatGender = kumiteCategoryDto.KumiteCatGender;
            existingKumiteCategory.KumiteCatAgeMin = kumiteCategoryDto.KumiteCatAgeMin;
            existingKumiteCategory.KumiteCatAgeMax = kumiteCategoryDto.KumiteCatAgeMax;
            existingKumiteCategory.KumiteCatWeightMin = kumiteCategoryDto.KumiteCatWeightMin;
            existingKumiteCategory.KumiteCatWeightMax = kumiteCategoryDto.KumiteCatWeightMax;

            _dbContext.KumiteCategories.Update(existingKumiteCategory);
            await _dbContext.SaveChangesAsync();
            KumiteCatChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
