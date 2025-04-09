using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IKumiteCategoryRepository
    {
        Task<List<KumiteCategoryDto>> GetAllKumiteCategoryAsync();
        Task<bool> UpdateKumiteCategoryAsync(KumiteCategoryDto kumiteCategory);
        Task<bool> AddKumiteCategoryAsync(KumiteCategoryDto kumiteCategory);
    }
}
