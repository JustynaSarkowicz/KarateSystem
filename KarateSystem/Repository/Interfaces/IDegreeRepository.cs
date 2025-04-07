using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Repository.Interfaces
{
    public interface IDegreeRepository
    {
        Task<List<Degree>> GetAllDegreeAsync();
        Task UpdateDegreeAsync(Degree degree);
        Task AddDegreeAsync(Degree degree);
    }
}
