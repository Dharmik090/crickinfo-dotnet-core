using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.ViewModels
{
    public class MatchUpdateViewModel
    {
        public string Result { get; set; } = string.Empty;

        public DateTime MatchDate { get; set; }

        public List<Team>? teamslist { get; set; }

        public int TeamA { get; set; } // ID of the first selected team
        public int TeamB { get; set; }
        public int MatchId { get; set; }
    }
}
