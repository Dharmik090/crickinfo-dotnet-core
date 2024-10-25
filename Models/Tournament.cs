using System.ComponentModel.DataAnnotations;


namespace crickinfo_mvc_ef_core.Models
{
	public class Tournament
	{
		public int Id { get; set; }  // Primary key

		[Required(ErrorMessage = "Tournament Name is required")]
		public string Name { get; set; } = String.Empty;

		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; } = String.Empty;

		[Required(ErrorMessage = "Tournament Date is required")]
		public DateTime DateOfTournament { get; set; }

		[Required(ErrorMessage = "Tournament Date is required")]
		public string Status { get; set; } = String.Empty;
		
		public int? UserId { get; set; }
		public User? User { get; set; }

		// Navigation property for many-to-many relationship with Team
		public ICollection<TeamTournament>? TeamTournaments { get; set; }
	}
} 
