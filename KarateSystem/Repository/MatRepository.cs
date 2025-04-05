using KarateSystem.Configurations;
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
    public class MatRepository : IMatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MatRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task AddMatAsync(Mat mat)
        {
            if (string.IsNullOrEmpty(mat.MatName)) return;
            var newMat = _dbContext.Mats.FirstOrDefault(c => c.MatId == mat.MatId || c.MatName == mat.MatName);
            if (newMat != null) return;
            _dbContext.Mats.Add(mat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Mat>> GetAllMatAsync()
        {
            return await _dbContext.Mats.ToListAsync();
        }

        public async Task UpdateMatAsync(Mat mat)
        {
            var existingMat = await _dbContext.Mats.FirstOrDefaultAsync(c => c.MatId == mat.MatId);
            if (string.IsNullOrEmpty(mat.MatName)) return;
            if (existingMat != null)
            {
                existingMat.MatName = mat.MatName;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
