using KarateSystem.Dto;
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
        Task<List<DegreeDto>> GetAllDegreeAsync();
        Task UpdateDegreeAsync(DegreeDto degreeDto);
        Task AddDegreeAsync(DegreeDto degreeDto);
        event EventHandler DegreesChanged;
    }
}
