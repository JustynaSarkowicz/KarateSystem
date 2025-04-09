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
    public class MatRepository : IMatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MatRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<bool> AddMatAsync(MatDto matDto)
        {
            if (string.IsNullOrEmpty(matDto.MatName)) return false;

            var mat = _mapper.Map<Mat>(matDto);

            var existingMat = await _dbContext.Mats
                .FirstOrDefaultAsync(c => c.MatId == mat.MatId || c.MatName == mat.MatName);
            if (existingMat != null) return false;

            _dbContext.Mats.Add(mat);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<MatDto>> GetAllMatAsync()
        {
            var mats = await _dbContext.Mats.AsNoTracking().ToListAsync();

            return _mapper.Map<List<MatDto>>(mats);
        }

        public async Task<bool> UpdateMatAsync(MatDto matDto)
        {
            if (string.IsNullOrEmpty(matDto.MatName)) return false;

            var existingMat = await _dbContext.Mats.FirstOrDefaultAsync(c => c.MatId == matDto.MatId);

            if (existingMat == null) return false;

            _mapper.Map(matDto, existingMat);
            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
