using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface ITourCatKataRepository
    {
        Task<List<TourCatKataDto>> GetCatKataByIdTourAsync(int tourId);
        Task DeleteCatKataFromTour(int tourCatKataId);
    }
}
