using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IKataRepository
    {
        Task<List<KataDto>> GetKatasByTourCatKataIdAsync(int tourCatKataId);
        Task UpdateGradesOnKataAsync(KataDto kata);
    }
}
