using Microsoft.EntityFrameworkCore;

namespace crickinfo_mvc_ef_core.Models.Interface
{
	public interface IUnitOfWork
	{
		ITournamentRepo Tournament {  get; }
		ITeamsRepo Team { get; }
        TeamTournament AddTeamTournament(TeamTournament teamTournament);
        //void AddTeamTournament(TeamTournament teamTournament);
        void Save();
	}
}
