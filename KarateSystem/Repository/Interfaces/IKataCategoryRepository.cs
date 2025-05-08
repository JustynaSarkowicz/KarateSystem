using KarateSystem.Dto;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IKataCategoryRepository
    {
        Task<List<KataCategoryDto>> GetAllKataCategoryAsync();
        Task UpdateKataCategoryAsync(KataCategoryDto kataCategory);
        Task AddKataCategoryAsync(KataCategoryDto kataCategory);
        Task DeleteKataCategoryAsync(int kataCatId);
    }
}
