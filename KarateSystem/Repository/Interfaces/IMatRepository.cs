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
        Task<List<Mat>> GetAllMatAsync();
        Task UpdateMatAsync(Mat mat);
        Task AddMatAsync(Mat mat);
    }
}
