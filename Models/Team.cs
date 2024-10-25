using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models
{
	public class Team
	{
		public Team() { }

		public Team(int id) { this.TeamId = id; }
		public int TeamId { get; set; }  // Primary key

		[Required(ErrorMessage = "Team name is required")]
		public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Team logo")]
        public string Logo { get; set; }

		// Navigation property for many-to-many relationship with Tournament
		public ICollection<TeamTournament>? TeamTournaments { get; set; }
	}
}
