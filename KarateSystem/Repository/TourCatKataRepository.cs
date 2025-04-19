using AutoMapper;
using KarateSystem.Configurations;
using KarateSystem.Dto;
using KarateSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository
{
    public class TourCatKataRepository : ITourCatKataRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TourCatKataRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<TourCatKataDto>> GetCatKataByIdTourAsync(int tourId)
        {
            var tourCatKata = await _dbContext.TourCatKatas.Where(t => t.TourId == tourId)
                .Include(t => t.KataCategory)
                    .ThenInclude(t => t.CatKataDegrees)
                        .ThenInclude(t => t.Degree)
                .Include(t => t.Mat)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TourCatKataDto>>(tourCatKata);
        }
    }
}
