using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface ITournamentRepository
    {
        Task AddTourAsync(TournamentDto tournament);
        Task<List<TournamentDto>> GetAllTournamentsAsync();
        Task UpdateTournamentAsync(TournamentDto tournament);
        Task<TournamentDto> GetLastFinishedTournament();
    }
}
