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
    public class TourCompetitorRepository : ITourCompetitorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TourCompetitorRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TourCompetitorDto>> GetTournamentsByCompetitorId(int competitorId)
        {
            return await _context.TourCompetitors
                .Where(tc => tc.CompId == competitorId)
                .Include(tc => tc.Tournament)
                .Include(tc => tc.Competitor)
                .Include(tc => tc.TourCatKata)
                .Include(tc => tc.TourCatKumite)
                .Select(tc => new TourCompetitorDto
                {
                    TourCompId = tc.TourCompId,
                    TourId = tc.TourId,
                    TourName = tc.Tournament.TourName,
                    TourDate = tc.Tournament.TourDate,
                    CompId = tc.CompId
                })
                .ToListAsync();
        }
    }
}
