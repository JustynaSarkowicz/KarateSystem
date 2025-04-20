using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface ITourCatKumiteRepository
    {
        Task DeleteCatKumiteFromTour(int tourCatKumiteId);
        Task AddCatKumiteToTour(TourCatKumiteDto tourCatKumiteDto);
        Task<List<TourCatKumiteDto>> GetCatKumiteByIdTourAsync(int tourId);
    }
}
