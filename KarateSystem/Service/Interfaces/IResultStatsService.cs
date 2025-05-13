using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Service.Interfaces
{
    public interface IResultStatsService
    {
        Task<Dictionary<string, int[]>> GetMedalStatsByClubAsync(int tourId);
        Task<Dictionary<string, int>> GetClubParticipationAsync(int tourId);
        Task<(int MaleCount, int FemaleCount)> GetGenderDistributionAsync(int tourId);
        Task<List<int>> GetCompetitorsAgesAsync(int tourId);
        Task<List<int>> GetFightDurationsAsync(int tourId);
        Task<Dictionary<string, int>> GetFightsPerCategoryAsync(int tourId);
        Task<Dictionary<string, int>> GetKatasPerCategoryAsync(int tourId);
        Task<List<decimal>> GetScoresInKatasAsync(int tourId);
    }
}
