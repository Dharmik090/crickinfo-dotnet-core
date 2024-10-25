using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using crickinfo_mvc_ef_core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using crickinfo_mvc_ef_core.ViewModels;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ITournamentRepo _tournamentRepo;
        private readonly IMatchesRepo _matchesRepo;
        private readonly IPointsTableRepo _pointsTableRepo;
        public TournamentController(ITournamentRepo tournamentRepo,IMatchesRepo matchesRepo, IPointsTableRepo pointsTableRepo)
        {
            _tournamentRepo = tournamentRepo;
            _matchesRepo = matchesRepo;
            _pointsTableRepo = pointsTableRepo;
        }

        public IActionResult Index() {
            IEnumerable<Tournament> model = _tournamentRepo.GetAllTournaments();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) {
                return RedirectToAction("Login", "User");
            }
			return View();
		}

        [HttpPost]
        public IActionResult Create(Tournament model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Tournament added successfully!";
                ViewBag.ModelStateValid = true;

                int userId = (int)HttpContext.Session.GetInt32("UserId");
                Tournament tournament = new Tournament
                {
                    Name = model.Name,
                    Description = model.Description,
                    DateOfTournament = model.DateOfTournament,
                    Status = model.Status,
                    UserId = userId,
                    User = new User((int)userId)
                };

                _tournamentRepo.Add(tournament, userId);

                 return RedirectToAction("Details", "User", new { id = userId });
            }

            // If model is invalid, show validation errors and return the view with the same data
          //  var errorMessages = ModelState.Values
          //.SelectMany(v => v.Errors)
          //.Select(e => e.ErrorMessage)
          //.ToList();

          //  // Join the error messages into a single string for display
          //  ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
          //  //ViewBag.ModelStateValid = false;
          //  //ViewBag.Message = $"There are {ModelState.ErrorCount} errors in the form. Please correct them.";

          //  ViewBag.ModelStateValid = false;
          //  //ModelState.AddModelError
          //  ViewBag.Message = "There are errors in the form. Please correct them."+ ModelState.ErrorCount.ToString()+" Errror" ;
           
            return View();
        }



        [HttpGet]
        public ViewResult Details(int id, string? messagePart, string? message)
        {

            Tournament tournament = _tournamentRepo.GetTournamentById(id);
            IEnumerable<Matches> matches = _matchesRepo.GetMatchesByTournamentId(id);
            IEnumerable<PointsTable> entries = _pointsTableRepo.GetPointsTableByTournamentId(id).Include(entry => entry.Team);

            TournamentDetailsViewModel model = new TournamentDetailsViewModel()
            {
                Tournament = tournament,
                Matches = matches,
                Entries = entries,
                Message = message,
                MessagePart = messagePart
            };

            if (model == null)
            {
                Response.StatusCode = 404;
                return View("Tournament Not Found", id);
            }
            return View(model);
        }


        public IActionResult Delete(int id) {
            _tournamentRepo.Delete(id);
            int? userId = HttpContext.Session.GetInt32("UserId");
            return RedirectToAction("Details", "User", new { id = userId.Value });
        }
    }
}
