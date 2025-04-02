using KarateSystem.Models;

namespace KarateSystem.Repository.Interfaces;

public interface ICompetitorRepository
{
    List<Competitor> GetAllCompetitors();
    Competitor? GetCompetitor(int compId);
    List<Tournament> GetCompetiorTournament(int compId);
    string GetCompeitorTourCatKata(int compId, int tourId);
    string GetCompeitorTourCatKumite(int compId, int tourId);
}
