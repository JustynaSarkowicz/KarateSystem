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
        Task<List<TourCompetitorDto>> GetTourCompetitorsByIdTourAsync(int tourId);
        Task<List<TourCompetitorDto>> GetCompetitorToursByIdCompAsync(int compId);
        Task DeleteTourComp(int tourCompId);
        Task AddCompToTour(TourCompetitorDto tourCompetitor);
    }
}
