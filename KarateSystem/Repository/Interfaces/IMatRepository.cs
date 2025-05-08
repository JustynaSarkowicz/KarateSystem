using KarateSystem.Dto;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IMatRepository
    {
        Task<List<MatDto>> GetAllMatAsync();
        Task UpdateMatAsync(MatDto matDto);
        Task AddMatAsync(MatDto matDto);
        Task DeleteMatAsync(int matId);
    }
}
