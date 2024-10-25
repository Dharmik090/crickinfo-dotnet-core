namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface ITournamentRepo
	{
		Tournament GetTournamentById(int id);
		IEnumerable<Tournament> GetTournamentByUserId(int id); 
		Tournament Add(Tournament tournament,int uid);
		IEnumerable<Tournament> GetAllTournaments();
		Tournament Update(Tournament tournament);
		Tournament Delete(int id);
		
	}
}
