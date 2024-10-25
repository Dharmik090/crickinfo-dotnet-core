using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crickinfo_mvc_ef_core.Models
{
	public class PointsTable
	{
		public int Id { get; set; }

		// Reference to the Team entity (foreign key)
		[Required(ErrorMessage = "Team is required")]
		public int TeamId { get; set; }   // Foreign key to Team

		public Team? Team { get; set; }     // Navigation property for Team

		[Required(ErrorMessage = "Team points are required")]
		public int Points { get; set; }

		[Required(ErrorMessage = "Team Net Run Rate is required")]
		[Column("NetRunRate")]
		public float NRR { get; set; }

		[Required(ErrorMessage = "Matches win is required")]
		public int Wins { get; set; }      // Use int for Wins, Lose, Draw (numeric data)

		[Required(ErrorMessage = "Matches lose is required")]
		public int Lose { get; set; }

		[Required(ErrorMessage = "Matches drawn is required")]
		public int Draw { get; set; }

		// Reference to the Tournament entity (foreign key)
		public int TournamentId { get; set; }
		public Tournament? Tournament { get; set; }
	}
}
