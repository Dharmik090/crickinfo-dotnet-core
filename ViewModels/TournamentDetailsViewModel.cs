using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.ViewModels
{
    public class TournamentDetailsViewModel
    {
        public Tournament Tournament { get; set; }
        public IEnumerable<PointsTable> Entries { get; set; }
        public IEnumerable<Matches> Matches { get; set; }

        public string? Message { get; set; }
        public string? MessagePart { get; set; }
    }
}
