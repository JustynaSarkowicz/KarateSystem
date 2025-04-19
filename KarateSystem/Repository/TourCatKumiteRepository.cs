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
    public class TourCatKumiteRepository : ITourCatKumiteRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TourCatKumiteRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<TourCatKumiteDto>> GetCatKumiteByIdTourAsync(int tourId)
        {
            var tourCatKumite = await _dbContext.TourCatKumites.Where(t => t.TourId == tourId)
                .Include(t => t.KumiteCategory)
                .Include(t => t.Mat)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TourCatKumiteDto>>(tourCatKumite);
        }
    }
}
