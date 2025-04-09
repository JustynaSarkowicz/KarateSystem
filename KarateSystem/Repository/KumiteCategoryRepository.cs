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
        public KumiteCategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<bool> AddKumiteCategoryAsync(KumiteCategoryDto kumiteCategoryDto)
        {
            if (string.IsNullOrWhiteSpace(kumiteCategoryDto.KumiteCatName) ||
                kumiteCategoryDto.KumiteCatWeightMin < 0 ||
                kumiteCategoryDto.KumiteCatWeightMax <= 0 ||
                kumiteCategoryDto.KumiteCatWeightMin > kumiteCategoryDto.KumiteCatWeightMax ||
                kumiteCategoryDto.KumiteCatAgeMin < 0 ||
                kumiteCategoryDto.KumiteCatAgeMax <= 0 ||
                kumiteCategoryDto.KumiteCatAgeMin > kumiteCategoryDto.KumiteCatAgeMax)
            {
                return false;
            }
            var existing = await _dbContext.KumiteCategories
                .FirstOrDefaultAsync(c => c.KumiteCatId == kumiteCategoryDto.KumiteCatId || c.KumiteCatName == kumiteCategoryDto.KumiteCatName);
            if (existing != null) return false;
            var kumiteCategory = _mapper.Map<KumiteCategory>(kumiteCategoryDto);
            try
            {
                _dbContext.KumiteCategories.Add(kumiteCategory);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<KumiteCategoryDto>> GetAllKumiteCategoryAsync()
        {
            var entities = await _dbContext.KumiteCategories.AsNoTracking().ToListAsync();
            return _mapper.Map<List<KumiteCategoryDto>>(entities);
        }

        public async Task<bool> UpdateKumiteCategoryAsync(KumiteCategoryDto kumiteCategoryDto)
        {
            var existingKumiteCategory = await _dbContext.KumiteCategories
                .FirstOrDefaultAsync(c => c.KumiteCatId == kumiteCategoryDto.KumiteCatId);

            if (existingKumiteCategory == null) return false;

            if (string.IsNullOrWhiteSpace(kumiteCategoryDto.KumiteCatName) ||
                kumiteCategoryDto.KumiteCatWeightMin < 0 ||
                kumiteCategoryDto.KumiteCatWeightMax <= 0 ||
                kumiteCategoryDto.KumiteCatWeightMin > kumiteCategoryDto.KumiteCatWeightMax ||
                kumiteCategoryDto.KumiteCatAgeMin < 0 ||
                kumiteCategoryDto.KumiteCatAgeMax <= 0 ||
                kumiteCategoryDto.KumiteCatAgeMin > kumiteCategoryDto.KumiteCatAgeMax)
            {
                return false;
            }

            existingKumiteCategory.KumiteCatName = kumiteCategoryDto.KumiteCatName;
            existingKumiteCategory.KumiteCatGender = kumiteCategoryDto.KumiteCatGender;
            existingKumiteCategory.KumiteCatAgeMin = kumiteCategoryDto.KumiteCatAgeMin;
            existingKumiteCategory.KumiteCatAgeMax = kumiteCategoryDto.KumiteCatAgeMax;
            existingKumiteCategory.KumiteCatWeightMin = kumiteCategoryDto.KumiteCatWeightMin;
            existingKumiteCategory.KumiteCatWeightMax = kumiteCategoryDto.KumiteCatWeightMax;
            
            try
            {
                _dbContext.KumiteCategories.Update(existingKumiteCategory);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
