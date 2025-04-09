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
        Task<bool> UpdateMatAsync(MatDto matDto);
        Task<bool> AddMatAsync(MatDto matDto);
    }
}
