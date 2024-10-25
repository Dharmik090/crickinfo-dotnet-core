
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models.CreateModels
{
    public class CreateMatchModel
    {
        public string Result { get; set; } = string.Empty; 

        public DateTime MatchDate { get; set; }

        public List<Team>? teamslist { get; set; }

        public IEnumerable<Matches>? matchlist { get; set; }

    }
}
