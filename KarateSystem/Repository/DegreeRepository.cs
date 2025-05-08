using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarateSystem.Repository
{
    public class DegreeRepository : IDegreeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DegreeRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task AddDegreeAsync(DegreeDto degreeDto)
        {
            var existingDegree = await _dbContext.Degrees.AnyAsync(u => u.DegreeName == degreeDto.DegreeName && u.DegreeId != degreeDto.DegreeId);

            if (existingDegree)
            {
                throw new Exception("Stopień o takiej nazwie już istnieje.");
            }

            var degree = _mapper.Map<Degree>(degreeDto);

            _dbContext.Degrees.Add(degree);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<DegreeDto>> GetAllDegreeAsync()
        {
            var degrees = await _dbContext.Degrees.AsNoTracking().ToListAsync();

            return _mapper.Map<List<DegreeDto>>(degrees);
        }

        public async Task UpdateDegreeAsync(DegreeDto degreeDto)
        {
            var existingDegree = await _dbContext.Degrees
                .FirstOrDefaultAsync(u => u.DegreeId == degreeDto.DegreeId);

            if (existingDegree == null)
                throw new Exception("Nie znaleziono stopnia do edycji.");

            var degreeTaken = await _dbContext.Degrees.AnyAsync(u => u.DegreeName == degreeDto.DegreeName && u.DegreeId != degreeDto.DegreeId);

            if (degreeTaken)
                throw new Exception("Stopień o takiej nazwoe już istnieje.");

            _mapper.Map(degreeDto, existingDegree);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDegreeAsync(int degreeId)
        {
            var existingDegree = await _dbContext.Degrees
                .FirstOrDefaultAsync(u => u.DegreeId == degreeId);

            if (existingDegree == null)
                throw new Exception("Nie znaleziono stopnia do usunięcia.");

            var isDegreeInUseComp = await _dbContext.Competitors
                .AnyAsync(u => u.CompDegreeId == degreeId);

            var isDegreeInUseCatKata = await _dbContext.CatKataDegrees
                .AnyAsync(u => u.DegreeId == degreeId);

            if (isDegreeInUseComp || isDegreeInUseCatKata)
                throw new Exception("Nie można usunąć stopnia, ponieważ jest używany przez zawodników lub kategorie kata.");

            _dbContext.Degrees.Remove(existingDegree);
            await _dbContext.SaveChangesAsync();
        }
    }
}
