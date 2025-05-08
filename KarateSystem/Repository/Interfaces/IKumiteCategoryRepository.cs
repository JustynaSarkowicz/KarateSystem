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
        Task UpdateKumiteCategoryAsync(KumiteCategoryDto kumiteCategory);
        Task AddKumiteCategoryAsync(KumiteCategoryDto kumiteCategory);
        Task DeleteKumiteCategoryAsync(int kumiteCatId);
    }
}
