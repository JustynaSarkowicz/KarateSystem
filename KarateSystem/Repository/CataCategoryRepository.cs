using KarateSystem.Configurations;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KarateSystem.Dto;

namespace KarateSystem.Repository
{
    public class CataCategoryRepository : ICataCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICatKataDegreeRepository _catKataDegreeRepository;

        public CataCategoryRepository(ApplicationDbContext context, IMapper mapper, ICatKataDegreeRepository catKataDegreeRepository)
        {
            _dbContext = context;
            _mapper = mapper;
            _catKataDegreeRepository = catKataDegreeRepository;
        }

        public async Task<bool> AddKataCategoryAsync(KataCategoryDto kataCategoryDto)
        {
            if (string.IsNullOrWhiteSpace(kataCategoryDto.KataCatName) ||
                kataCategoryDto.KataCatAgeMin < 0 ||
                kataCategoryDto.KataCatAgeMax <= 0 ||
                kataCategoryDto.KataCatAgeMin > kataCategoryDto.KataCatAgeMax ||
                kataCategoryDto.CatKataDegrees == null || !kataCategoryDto.CatKataDegrees.Any())
            {
                return false;
            }

            var existing = await _dbContext.KataCategories
                .FirstOrDefaultAsync(c => c.KataCatId == kataCategoryDto.KataCatId || c.KataCatName == kataCategoryDto.KataCatName);
            if (existing != null) return false;

            var kataCategory = _mapper.Map<KataCategory>(kataCategoryDto);
            kataCategory.CatKataDegrees = kataCategoryDto.CatKataDegrees
                .Select(d => new CatKataDegree
                {
                    DegreeId = d.DegreeId
                }).ToList();

            try
            {
                _dbContext.KataCategories.Add(kataCategory);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<KataCategoryDto>> GetAllKataCategoryAsync()
        {
            var entities = await _dbContext.KataCategories
                .Include(k => k.CatKataDegrees)
                    .ThenInclude(cd => cd.Degree)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<KataCategoryDto>>(entities);
        }

        public async Task<bool> UpdateKataCategoryAsync(KataCategoryDto kataCategoryDto)
        {
            var existingKataCategory = await _dbContext.KataCategories
                .Include(k => k.CatKataDegrees)
                .FirstOrDefaultAsync(k => k.KataCatId == kataCategoryDto.KataCatId);

            if (string.IsNullOrWhiteSpace(kataCategoryDto.KataCatName) ||
                kataCategoryDto.KataCatAgeMin < 0 ||
                kataCategoryDto.KataCatAgeMax <= 0 ||
                kataCategoryDto.KataCatAgeMin > kataCategoryDto.KataCatAgeMax ||
                kataCategoryDto.CatKataDegrees == null || !kataCategoryDto.CatKataDegrees.Any())
            {
                return false;
            }

            if (existingKataCategory == null) return false;

            existingKataCategory.KataCatName = kataCategoryDto.KataCatName;
            existingKataCategory.KataCatGender = kataCategoryDto.KataCatGender;
            existingKataCategory.KataCatAgeMin = kataCategoryDto.KataCatAgeMin;
            existingKataCategory.KataCatAgeMax = kataCategoryDto.KataCatAgeMax;

            foreach (var degreeDto in kataCategoryDto.CatKataDegrees)
            {
                var existingDegree = existingKataCategory.CatKataDegrees
                    .FirstOrDefault(d => d.DegreeId == degreeDto.DegreeId);

                if (existingDegree == null)
                {
                    await _catKataDegreeRepository.AddDegreeToCategoryAsync(kataCategoryDto.KataCatId, degreeDto.DegreeId);
                }
            }

            var existingDegreeIds = kataCategoryDto.CatKataDegrees.Select(d => d.DegreeId).ToList();
            foreach (var degree in existingKataCategory.CatKataDegrees.ToList())
            {
                if (!existingDegreeIds.Contains(degree.DegreeId))
                {
                    await _catKataDegreeRepository.RemoveDegreeFromCategoryAsync(kataCategoryDto.KataCatId, degree.DegreeId);
                }
            }

            try
            {
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
