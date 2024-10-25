using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.ViewModels
{
    public class UserTournamentViewModel
    {
        public User User { get; set; }
        public IEnumerable<Tournament> Tournaments { get; set; }
    }
}
