using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces;
public interface IClubRepository
{
    Task<List<Club>> GetAllClubsAsync();
    Task<Club> GetClubAsync(int clubId);
    Task UpdateClubAsync(Club club);
    Task AddClubAsync(Club club);
}

