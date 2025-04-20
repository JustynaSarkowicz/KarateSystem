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
    public class KataCategoryRepository : IKataCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICatKataDegreeRepository _catKataDegreeRepository;
        public event EventHandler KataCatChanged;

        public KataCategoryRepository(ApplicationDbContext context, IMapper mapper, ICatKataDegreeRepository catKataDegreeRepository)
        {
            _dbContext = context;
            _mapper = mapper;
            _catKataDegreeRepository = catKataDegreeRepository;
        }

        public async Task AddKataCategoryAsync(KataCategoryDto kataCategoryDto)
        {
            var existingKataCat = await _dbContext.KataCategories
                .AnyAsync(c => c.KataCatId != kataCategoryDto.KataCatId && c.KataCatName == kataCategoryDto.KataCatName);

            if (existingKataCat)
            {
                throw new Exception("Kategoria kata o tej nazwie już istnieje.");
            }

            var kataCategory = _mapper.Map<KataCategory>(kataCategoryDto);

            kataCategory.CatKataDegrees = kataCategoryDto.CatKataDegrees
                .Select(d => new CatKataDegree
                {
                    DegreeId = d.DegreeId
                }).ToList();
            
            _dbContext.KataCategories.Add(kataCategory);
            await _dbContext.SaveChangesAsync();

            KataCatChanged?.Invoke(this, EventArgs.Empty);
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

        public async Task UpdateKataCategoryAsync(KataCategoryDto kataCategoryDto)
        {
            var existingKataCategory = await _dbContext.KataCategories
                .Include(k => k.CatKataDegrees)
                .FirstOrDefaultAsync(k => k.KataCatId == kataCategoryDto.KataCatId);

            if (existingKataCategory == null)
                throw new Exception("Nie znaleziono kategorii kata.");

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

            await _dbContext.SaveChangesAsync();

            KataCatChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
