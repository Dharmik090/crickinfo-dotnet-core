using crickinfo_mvc_ef_core.Models;

namespace crickinfo_mvc_ef_core.ViewModels
{
    public class PointsTableTournamentViewModel
    {
        public IEnumerable<PointsTable> pointsTables { get; set; }
        public Tournament Tournament { get; set; }
    }
}
