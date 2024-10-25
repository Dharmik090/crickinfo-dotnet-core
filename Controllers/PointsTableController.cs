using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace crickinfo_mvc_ef_core.Controllers
{
    public class PointsTableController : Controller
    {
        private readonly IPointsTableRepo _pointTablerepo;
        private readonly ITournamentRepo _tournamentRepo;
        private readonly ITeamsRepo _teamRepo;

        public PointsTableController(IPointsTableRepo pointtablerepo, ITournamentRepo tournamentRepo, ITeamsRepo teamRepo)
        {
            _pointTablerepo = pointtablerepo;
            _tournamentRepo = tournamentRepo;
            _teamRepo = teamRepo;
        }

        [HttpGet]
        public IActionResult Create(PointsTable model)
        {
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Details(int id) {
            IEnumerable<PointsTable> entries = _pointTablerepo.GetPointsTableByTournamentId(id).Include(entry => entry.Team);
            Tournament tournamet = _tournamentRepo.GetTournamentById(id);

            PointsTableTournamentViewModel model = new PointsTableTournamentViewModel()
            {
                pointsTables = entries,
                Tournament = tournamet
            };

            return View(model);
        }
    }
}
