namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface ITeamsRepo
	{
		Team GetTeamById(int id);
		Team Add(Team team,int tournament_id);
		IEnumerable<Team> GetAllTeams();
		IEnumerable<Team> GetTeamsByTournamentId(int tournament_id);
        Team Update(Team team);
		Team Delete(int id);

	}
}
