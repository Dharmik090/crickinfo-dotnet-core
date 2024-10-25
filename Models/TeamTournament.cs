using crickinfo_mvc_ef_core.Models;
using System.Numerics;

public class TeamTournament
{
	public int Id { get; set; }	
	public int TeamId { get; set; }
	public Team? Team { get; set; }

	public int TournamentId { get; set; }
	public Tournament? Tournament { get; set; }

	// Additional data for the relationship
	public DateTime? DateJoined { get; set; }
	public string? TeamStatus { get; set; }  // Example: "Participating", "Eliminated"
}
