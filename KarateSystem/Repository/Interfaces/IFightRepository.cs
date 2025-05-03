using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IFightRepository
    {
        Task<bool> SetFightsAsync(int tournamentId);
        Task<List<FightDto>> GetFightsByTourAsync(int tournamentId);
        Task UpdateFightsAsync(FightDto fightDto);
    }
}
