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

        public async Task<bool> UpdateClubAsync(ClubDto clubDto)
        {
            if (string.IsNullOrEmpty(clubDto.ClubName) || string.IsNullOrEmpty(clubDto.ClubPlace)) return false;

            var existingClub = await _dbContext.Clubs
                .FirstOrDefaultAsync(c => c.ClubId == clubDto.ClubId);

            if (existingClub == null) return false;
            
            _mapper.Map(clubDto, existingClub);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddClubAsync(ClubDto clubDto)
        {
            if (string.IsNullOrEmpty(clubDto.ClubName) || string.IsNullOrEmpty(clubDto.ClubPlace)) return false;

            var newClub = _dbContext.Clubs
                .FirstOrDefault(c => c.ClubId == clubDto.ClubId || c.ClubName == clubDto.ClubName);

            if (newClub != null) return false;

            var club = _mapper.Map<Club>(clubDto);
            _dbContext.Clubs.Add(club);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
