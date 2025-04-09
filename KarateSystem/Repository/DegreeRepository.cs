using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddDegreeAsync(DegreeDto degreeDto)
        {
            if (string.IsNullOrEmpty(degreeDto.DegreeName)) return false;

            var degree = _mapper.Map<Degree>(degreeDto);

            var newDegree = await _dbContext.Degrees
                .FirstOrDefaultAsync(c => c.DegreeId == degree.DegreeId || c.DegreeName == degree.DegreeName);
            if (newDegree != null) return false;

            _dbContext.Degrees.Add(degree);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<DegreeDto>> GetAllDegreeAsync()
        {
            var degrees = await _dbContext.Degrees.AsNoTracking().ToListAsync();

            return _mapper.Map<List<DegreeDto>>(degrees);
        }

        public async Task<bool> UpdateDegreeAsync(DegreeDto degreeDto)
        {
            if (string.IsNullOrEmpty(degreeDto.DegreeName)) return false;

            var existingDegree = await _dbContext.Degrees
                .FirstOrDefaultAsync(c => c.DegreeId == degreeDto.DegreeId);

            if (existingDegree == null) return false;

            _mapper.Map(degreeDto, existingDegree);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
