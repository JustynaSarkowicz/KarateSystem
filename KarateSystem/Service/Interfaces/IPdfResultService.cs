using KarateSystem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Service.Interfaces
{
    public interface IPdfResultService
    {
        Task<List<KataResultDto>> GetKataResultsAsync(int tourId);
    }
}
