using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace crickinfo_mvc_ef_core.Models.CreateModels
{
    public class CreatePointsTableModel
    {

        public Team? team { get; set; }
        public int Points { get; set; }

        public float NRR { get; set; }

        public int Wins { get; set; }      // Use int for Wins, Lose, Draw (numeric data)

        public int Lose { get; set; }

        public int Draw { get; set; }

    }
}
