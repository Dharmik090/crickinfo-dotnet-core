using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.CreateModels;
using Microsoft.AspNetCore.Mvc;
using crickinfo_mvc_ef_core.ViewModels;


namespace crickinfo_mvc_ef_core.Controllers
{
    public class MatchesController : Controller
    {

        private readonly IMatchesRepo _matchRepo;
        private readonly ITeamsRepo _teamRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITournamentRepo _tournamentRepo;
        private readonly IPointsTableRepo _pointsTableRepo;
        private readonly ILogger<MatchesController> _logger;


        public MatchesController(IMatchesRepo matchRepo, IUnitOfWork unitOfWork, ITeamsRepo teamRepo, ITournamentRepo tournamentRepo, ILogger<MatchesController> logger, IPointsTableRepo pointsTableRepo)
        {
            _matchRepo = matchRepo;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _teamRepo = teamRepo ?? throw new ArgumentException(nameof(teamRepo));
            _tournamentRepo = tournamentRepo ?? throw new ArgumentException(nameof(tournamentRepo));
            _logger = logger;
            _pointsTableRepo = pointsTableRepo;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            HttpContext.Session.SetInt32("TournamentId", id);
            var teams = _unitOfWork.Team.GetTeamsByTournamentId(id).ToList();
            var matches = _matchRepo.GetMatchesByTournamentId(id);

            var model = new CreateMatchModel
            {
                teamslist = teams,
                matchlist = matches
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateMatchModel model, int[] selectedTeamIds)
        {
            int? tournament_id = HttpContext.Session.GetInt32("TournamentId");
            var teams = _unitOfWork.Team.GetTeamsByTournamentId(tournament_id.Value).ToList();
            var matches = _matchRepo.GetMatchesByTournamentId(tournament_id.Value).ToList();

            model.teamslist = teams;
            model.matchlist = matches;

            if (ModelState.IsValid)
            {
                // Validate that exactly two teams are selected
                if (selectedTeamIds.Length != 2)
                {
                    ModelState.AddModelError(string.Empty, "You must select exactly two teams.");
                    return View(model);
                }

                // Fetch the selected teams
                var teamA = _teamRepo.GetTeamById(selectedTeamIds[0]);
                var teamB = _teamRepo.GetTeamById(selectedTeamIds[1]);
                Matches match = new Matches
                {
                    TeamAId = selectedTeamIds[0],
                    TeamBId = selectedTeamIds[1],
                    TeamA = teamA,
                    TeamB = teamB,
                    Result = model.Result,
                    MatchDate = model.MatchDate,
                    Tournament = _tournamentRepo.GetTournamentById(tournament_id.Value),
                    TournamentId = tournament_id.Value
                };


                // Add match to the database
                _matchRepo.Add(match, tournament_id.Value);
                PointsTable entryA = _pointsTableRepo.GetPointsTableByTeamIdAndTournamentId(match.TeamAId, tournament_id.Value);
                PointsTable entryB = _pointsTableRepo.GetPointsTableByTeamIdAndTournamentId(match.TeamBId, tournament_id.Value);

                if (match.Result == match.TeamA.Name)
                {
                    entryA.Wins = entryA.Wins + 1;
                    entryA.Points = entryA.Points + 2;

                    entryB.Lose = entryB.Lose + 1;
                }
                else if (match.Result == match.TeamB.Name)
                {
                    entryB.Wins = entryB.Wins + 1;
                    entryB.Points = entryB.Points + 2;

                    entryA.Lose = entryA.Lose + 1;
                }

                _pointsTableRepo.Update(entryA);
                _pointsTableRepo.Update(entryB);

                model.matchlist = _matchRepo.GetMatchesByTournamentId(tournament_id.Value).ToList();

                return RedirectToAction("Create", new { id = tournament_id.Value });
            }

            // If ModelState is not valid, capture errors and display them
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            ViewBag.ErrorMessages = string.Join("<br/>", errorMessages);
            ViewBag.ModelStateValid = false;
            ViewBag.Message = "There are errors in the form. Please correct them.";

            // Return the view with the model (and reloaded team list)
            model.teamslist = teams;
            model.matchlist = matches;
            return View(model);
        }




        [HttpGet]
        public IActionResult Update(int id)
        {
            HttpContext.Session.SetInt32("MatchId", id);

            var match = _matchRepo.GetMatchesById(id);
            HttpContext.Session.SetString("Result", match.Result);
            var teams = _unitOfWork.Team.GetTeamsByTournamentId(match.TournamentId).ToList();

            var model = new MatchUpdateViewModel
            {
                teamslist = teams,
                MatchDate = match.MatchDate,
                Result = match.Result,
                TeamA = match.TeamAId,
                TeamB = match.TeamBId,
                MatchId = match.Id
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(MatchUpdateViewModel model, int[] selectedTeamIds)
        {
            int? tournament_id = HttpContext.Session.GetInt32("TournamentId");
            var teams = _unitOfWork.Team.GetTeamsByTournamentId(tournament_id.Value).ToList();
            int? matchId = HttpContext.Session.GetInt32("MatchId");
            var match = _matchRepo.GetMatchesById(matchId.Value);

            model.teamslist = teams;

            if (ModelState.IsValid)
            {
                // Validate that exactly two teams are selected
                if (selectedTeamIds.Length != 2)
                {
                    ModelState.AddModelError(string.Empty, "You must select exactly two teams.");
                    return View(model);
                }

                // Fetch the selected teams
                var teamA = _teamRepo.GetTeamById(selectedTeamIds[0]);
                var teamB = _teamRepo.GetTeamById(selectedTeamIds[1]);

                // Update match details
                match.TeamAId = selectedTeamIds[0];
                match.TeamBId = selectedTeamIds[1];
                match.Result = model.Result;
                match.MatchDate = model.MatchDate;

                _matchRepo.Update(matchId.Value, match);

                // Handle points table update if the result has changed
                if (match.Result != HttpContext.Session.GetString("Result"))
                {
                    var entryA = _pointsTableRepo.GetPointsTableByTeamIdAndTournamentId(match.TeamAId, tournament_id.Value);
                    var entryB = _pointsTableRepo.GetPointsTableByTeamIdAndTournamentId(match.TeamBId, tournament_id.Value);

                    if (HttpContext.Session.GetString("Result") == match.TeamA.Name)
                    {
                        entryA.Wins--;
                        entryA.Points -= 2;
                        entryA.Lose++;
                    }
                    else if (HttpContext.Session.GetString("Result") == match.TeamB.Name)
                    {
                        entryB.Wins--;
                        entryB.Points -= 2;
                        entryB.Lose++;
                    }

                    if (match.Result == match.TeamA.Name)
                    {
                        entryA.Wins++;
                        entryA.Points += 2;
                        entryA.Lose--;
                    }
                    else if (match.Result == match.TeamB.Name)
                    {
                        entryB.Wins++;
                        entryB.Points += 2;
                        entryB.Lose--;
                    }

                    _pointsTableRepo.Update(entryA);
                    _pointsTableRepo.Update(entryB);
                }

                return RedirectToAction("Create", new { id = tournament_id.Value });
            }

            // If ModelState is not valid, display errors
            model.teamslist = teams;
            return View(model);
        }
    }
}