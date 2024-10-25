namespace crickinfo_mvc_ef_core.Models.Repository
{
	public interface ITeamRepo
	{
		Team GetTeamById(int id);
		IEnumerable<Team> GetAllTeams();
		Team Add(Team team);	
		Team Update(Team team);
		Team Delete(int id);
		IEnumerable<Team> GetTeamsByTournamentId(int tid);

	}
}
