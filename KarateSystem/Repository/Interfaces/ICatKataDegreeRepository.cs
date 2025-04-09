using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface ICatKataDegreeRepository
    {
        Task AddDegreeToCategoryAsync(int categoryId, int degreeId);
        Task RemoveDegreeFromCategoryAsync(int categoryId, int degreeId);
    }
}
