namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface IMatchesRepo
	{
		Matches GetMatchesById(int id);
		IEnumerable<Matches> GetMatchesByTournamentId(int id);
		Matches Add(Matches matches, int tournamentId);
		IEnumerable<Matches> GetAllMatches();
		Matches Update(int id,Matches matches);
		Matches Delete(int id);
	}
}
