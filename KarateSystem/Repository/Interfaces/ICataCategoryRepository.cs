using KarateSystem.Dto;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface ICataCategoryRepository
    {
        Task<List<KataCategoryDto>> GetAllKataCategoryAsync();
        Task<bool> UpdateKataCategoryAsync(KataCategoryDto kataCategory);
        Task<bool> AddKataCategoryAsync(KataCategoryDto kataCategory);
    }
}
