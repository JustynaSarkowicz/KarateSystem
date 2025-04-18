using KarateSystem.Dto;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces;
public interface IClubRepository
{
    Task<List<ClubDto>> GetAllClubsAsync();
    Task<ClubDto> GetClubAsync(int clubId);
    Task UpdateClubAsync(ClubDto clubDto);
    Task AddClubAsync(ClubDto clubDto);
}

