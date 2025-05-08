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
    public class MatRepository : IMatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MatRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task AddMatAsync(MatDto matDto)
        {
            var existingMat = await _dbContext.Mats.AnyAsync(c => c.MatId != matDto.MatId && c.MatName == matDto.MatName);
            
            if (existingMat)
            {
                throw new Exception("Mata o takiej nazwie już istnieje.");
            }

            var newMat = _mapper.Map<Mat>(matDto);

            _dbContext.Mats.Add(newMat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MatDto>> GetAllMatAsync()
        {
            var mats = await _dbContext.Mats.AsNoTracking().ToListAsync();

            return _mapper.Map<List<MatDto>>(mats);
        }

        public async Task UpdateMatAsync(MatDto matDto)
        {
            var existingMat = await _dbContext.Mats.FirstOrDefaultAsync(c => c.MatId == matDto.MatId);

            if (existingMat == null)
                throw new Exception("Nie znaleziono maty do edycji.");
            
            var matTaken = await _dbContext.Mats.AnyAsync(u => u.MatName == matDto.MatName && u.MatId != matDto.MatId);

            if (matTaken)
                throw new Exception("Mata o takiej nazwie już istnieje.");

            _mapper.Map(matDto, existingMat);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteMatAsync(int matId)
        {
            var mat = await _dbContext.Mats
                .FirstOrDefaultAsync(m => m.MatId == matId);

            if (mat == null)
                throw new Exception("Nie znaleziono maty.");

            var isMatInUseCatKata = await _dbContext.TourCatKatas
                .AnyAsync(c => c.MatId == matId); 
            var isMatInUseCatKumite = await _dbContext.TourCatKumites
                .AnyAsync(c => c.MatId == matId); 

            if (isMatInUseCatKata || isMatInUseCatKumite)
                throw new Exception("Nie można usunąć maty, ponieważ jest powiązana z kategoriami kata lub kumite.");

            _dbContext.Mats.Remove(mat);
            await _dbContext.SaveChangesAsync();
        }

    }
}
