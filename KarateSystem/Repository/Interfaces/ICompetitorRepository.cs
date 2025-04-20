using KarateSystem.Dto;
using KarateSystem.Models;

namespace KarateSystem.Repository.Interfaces;

public interface ICompetitorRepository
{
    Task<List<CompetitorDto>> GetAllCompetitorsAsync();
    Task UpdateCompAsync(CompetitorDto competitor);
    Task AddCompAsync(CompetitorDto competitor);
    public event EventHandler CompChanged;
}
