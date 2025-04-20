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
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ClubRepository(ApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public async Task<List<ClubDto>> GetAllClubsAsync()
        {
            var clubs = await _dbContext.Clubs.AsNoTracking().ToListAsync();
            return _mapper.Map<List<ClubDto>>(clubs); 
        }
        public async Task<ClubDto> GetClubAsync(int clubId)
        {
            var club = await _dbContext.Clubs
                .Where(c => c.ClubId == clubId)
                .FirstOrDefaultAsync();
            return _mapper.Map<ClubDto>(club); 
        }

        public async Task UpdateClubAsync(ClubDto clubDto)
        {
            var existingClub = await _dbContext.Clubs
                .FirstOrDefaultAsync(c => c.ClubId == clubDto.ClubId);

            if (existingClub == null)
                throw new Exception("Nie znaleziono klubu do edycji.");

            var clubTaken = await _dbContext.Clubs.AnyAsync(u => u.ClubName == clubDto.ClubName && u.ClubId != clubDto.ClubId);

            if (clubTaken)
                throw new Exception("Klub o takiej nazwie już istnieje.");

            _mapper.Map(clubDto, existingClub);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddClubAsync(ClubDto clubDto)
        {
            var existingClub = await _dbContext.Clubs.AnyAsync(c => c.ClubId != clubDto.ClubId && c.ClubName == clubDto.ClubName);

            if (existingClub)
            {
                throw new Exception("Klub o takiej nazwie już istnieje.");
            }

            var newClub = _mapper.Map<Club>(clubDto);

            _dbContext.Clubs.Add(newClub);
            await _dbContext.SaveChangesAsync();
        }
    }
}
