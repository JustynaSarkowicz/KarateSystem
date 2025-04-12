using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface ITourCompetitorRepository
    {
        Task<List<TourCompetitorDto>> GetTournamentsByCompetitorId(int competitorId);
    }
}
