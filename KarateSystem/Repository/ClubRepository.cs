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
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ClubRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task<List<Club>> GetAllClubsAsync()
        {
            return await _dbContext.Clubs.ToListAsync();
        }
        public async Task<Club> GetClubAsync(int clubId)
        {
            var club = await _dbContext.Clubs
                .Where(c => c.ClubId == clubId)
                .FirstOrDefaultAsync();
            return club;
        }
        public async Task UpdateClubAsync(Club club)
        {
            var existingClub = await _dbContext.Clubs.FindAsync(club.ClubId);
            if (existingClub != null)
            {
                existingClub.ClubName = club.ClubName;
                existingClub.ClubPlace = club.ClubPlace;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
