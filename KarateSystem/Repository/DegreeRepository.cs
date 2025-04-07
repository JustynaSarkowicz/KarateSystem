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
    public class DegreeRepository : IDegreeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DegreeRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task AddDegreeAsync(Degree degree)
        {
            if(string.IsNullOrEmpty(degree.DegreeName)) return;
            var newDegree = _dbContext.Degrees.FirstOrDefault(c => c.DegreeId == degree.DegreeId || c.DegreeName == degree.DegreeName);
            if (newDegree != null) return;
            _dbContext.Degrees.Add(degree);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Degree>> GetAllDegreeAsync()
        {
            return await _dbContext.Degrees.ToListAsync();
        }

        public async Task UpdateDegreeAsync(Degree degree)
        {
            var existingDegree = _dbContext.Degrees.FirstOrDefault(c => c.DegreeId == degree.DegreeId);
            if (string.IsNullOrEmpty(degree.DegreeName)) return;
            if (existingDegree != null)
            {
                existingDegree.DegreeName = degree.DegreeName;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
