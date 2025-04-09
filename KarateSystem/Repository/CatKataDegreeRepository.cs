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
    public class CatKataDegreeRepository : ICatKataDegreeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CatKataDegreeRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddDegreeToCategoryAsync(int categoryId, int degreeId)
        {
            var category = await _dbContext.KataCategories
                .Include(c => c.CatKataDegrees)
                .FirstOrDefaultAsync(c => c.KataCatId == categoryId);

            var degree = await _dbContext.Degrees.FindAsync(degreeId);

            if (category != null && degree != null)
            {
                category.CatKataDegrees.Add(new CatKataDegree { DegreeId = degreeId });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveDegreeFromCategoryAsync(int categoryId, int degreeId)
        {
            var category = await _dbContext.KataCategories
                .Include(c => c.CatKataDegrees)
                .ThenInclude(cd => cd.Degree)
                .FirstOrDefaultAsync(c => c.KataCatId == categoryId);
            if (category != null)
            {
                var itemToRemove = await _dbContext.CatKataDegrees
                    .FirstOrDefaultAsync(cd => cd.KataCatId == categoryId && cd.DegreeId == degreeId);

                if (itemToRemove != null)
                {
                    _dbContext.CatKataDegrees.Remove(itemToRemove);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
