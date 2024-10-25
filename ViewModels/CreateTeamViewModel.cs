using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.ViewModels
{
    public class CreateTeamViewModel
    {
        public Team? Team { get; set; }
        public IEnumerable<Team>? TeamList { get; set; }
    }
}
