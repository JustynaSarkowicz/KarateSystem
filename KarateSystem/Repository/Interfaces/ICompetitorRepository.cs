using KarateSystem.Models;

namespace KarateSystem.Repository.Interfaces;

public interface ICompetitorRepository
{
    Task<List<Competitor>> GetAllCompetitorsAsync();
    Task<Competitor?> GetCompetitorAsync(int compId);
    Task<List<Tournament>> GetCompetitorTournamentAsync(int compId);
    Task<string> GetCompetitorTourCatKataAsync(int compId, int tourId);
    Task<string> GetCompetitorTourCatKumiteAsync(int compId, int tourId);
}
