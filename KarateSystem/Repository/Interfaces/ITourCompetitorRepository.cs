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
        Task<List<TourCompetitorDto>> GetCompetitorToursByCatKataIdAsync(int catKataId);
        Task<List<TourCompetitorDto>> GetCompetitorToursByCatKumiteIdAsync(int catKumiteId);
        Task AddCompToTourCatKata(TourCompetitorDto tourCompetitor, int kataCatId);
        Task AddCompToTourCatKumite(TourCompetitorDto tourCompetitor, int kumiteCatId);
        Task DeleteCompFromTourCatKumite(TourCompetitorDto tourCompetitor);
        Task DeleteCompFromTourCatKata(TourCompetitorDto tourCompetitor);
        Task<string> SetCompToCatKataAutomatic(int tourId);
        Task<string> SetCompToKumiteCatAutomatic(int tourId);
    }
}
