using KarateSystem.Dto;
using KarateSystem.Models;

namespace KarateSystem.Repository.Interfaces;

public interface ICompetitorRepository
{
    Task<List<CompetitorDto>> GetAllCompetitorsAsync();
    Task<bool> UpdateCompAsync(CompetitorDto competitor);
    Task<bool> AddCompAsync(CompetitorDto competitor);
}
