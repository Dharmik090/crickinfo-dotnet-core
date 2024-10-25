using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using crickinfo_mvc_ef_core.ViewModels;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamsRepo _teamRepo;
        private readonly ITournamentRepo _tournamentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPointsTableRepo _pointsTableRepo;
        public TeamController(ITeamsRepo teamRepo,IUnitOfWork unitOfWork, ITournamentRepo tournamentRepo, IPointsTableRepo pointsTableRepo)
        {
            _teamRepo = teamRepo;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _tournamentRepo = tournamentRepo;
            _pointsTableRepo = pointsTableRepo;
        }

        public IActionResult Index()
        {
            var teams = _unitOfWork.Team.GetAllTeams();
            return View(teams);
        }

        [HttpGet]
        public ViewResult Create(int id)
        {
            HttpContext.Session.SetInt32("TournamentId", id);
            IEnumerable<Team> list = _teamRepo.GetTeamsByTournamentId(id);
            CreateTeamViewModel model = new CreateTeamViewModel() { TeamList = list };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateTeamViewModel model)
        {
            int? tournamentId = HttpContext.Session.GetInt32("TournamentId");
            model.Team.TeamTournaments = new List<TeamTournament>();

            if (tournamentId == null) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                Team team = new Team
                {
                    Name = model.Team.Name,
                    Logo = model.Team.Logo,
                };
                

                //_teamRepo.Add(team,tournament_id); 
                Console.WriteLine($"ID1 = {tournamentId.Value}");
                _unitOfWork.Team.Add(team, tournamentId.Value);
                _unitOfWork.Save();

                PointsTable entry = new PointsTable()
                {
                    TeamId = team.TeamId,
                    Team = team,
                    Points = 0,
                    NRR = 0.0F,
                    Wins = 0,
                    Lose = 0,
                    Draw = 0,
                    TournamentId = tournamentId.Value,
                    Tournament = _tournamentRepo.GetTournamentById(tournamentId.Value)
                };

                _pointsTableRepo.Add(entry);

                model.TeamList = _teamRepo.GetTeamsByTournamentId(tournamentId.Value);
                int? userId = HttpContext.Session.GetInt32("UserId");
                return RedirectToAction("Create","Team", new { id = tournamentId.Value });
            }

            IEnumerable<Team> list = _teamRepo.GetTeamsByTournamentId(tournamentId.Value);
            model.TeamList = list;

            //var errorMessages = ModelState.Values
            //.SelectMany(v => v.Errors)
            //.Select(e => e.ErrorMessage)
            //.ToList();

            //ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
            //ViewBag.ModelStateValid = false;
            //ViewBag.Message = "There are errors in the form. Please correct them." + ModelState.ErrorCount.ToString() + " Errror";
            return View();
        }


    }

    //in view
    /*
     @if (ViewBag.ModelStateValid == true)
         {
             <div class="alert alert-success">@ViewBag.Message</div>
         }
         else if (ViewBag.ModelStateValid == false)
         {
             <div class="alert alert-danger">@ViewBag.Message</div>
         }
         <hr />

         @if (ViewBag.ModelStateValid == false)
         {
             <div class="alert alert-danger">
                 <strong>Errors:</strong><br />
                 @Html.Raw(ViewBag.ErrorMessages) <!-- Display error messages -->
             </div>
         }
     */
}
