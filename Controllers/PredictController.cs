using crickinfo_mvc_ef_core.Models;

using crickinfo_mvc_ef_core.Models.Algorithms;
using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace crickinfo_mvc_ef_core.Controllers
{
    public class PredictController : Controller
    {
        public readonly ITournamentRepo _tournamentRepo;
        public readonly IPointsTableRepo _pointsTableRepo;
        public readonly ITeamsRepo _teamsRepo;
        public readonly IMatchesRepo _matchesRepo;

        public PredictController(ITournamentRepo tournamentRepo, IPointsTableRepo pointsTableRepo, ITeamsRepo teamsRepo, IMatchesRepo matchesRepo)
        {
            _tournamentRepo = tournamentRepo;
            _pointsTableRepo = pointsTableRepo;
            _teamsRepo = teamsRepo;
            _matchesRepo = matchesRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tournaments = _tournamentRepo.GetAllTournaments().ToList();
            return View(tournaments);
        }


        public IActionResult RenderTeams(int tournamentId)
        {
            //PointsTable pt = _pointsTableRepo.GetPointTableById(tournament_id);
            var teams = _teamsRepo.GetTeamsByTournamentId(tournamentId).ToList();
            ViewBag.TournamentId = tournamentId;  // Pass additional value via ViewBag
            return View(teams);
        }

        [HttpPost]
        public IActionResult ExecutePredict(int teamId, int tournamentId)
        {

            Team t1 = _teamsRepo.GetTeamById(teamId);

            int ppm = 2;

            PointsTable pt1 = _pointsTableRepo.GetPointsTableByTeamIdAndTournamentId(teamId, tournamentId);

            var mt1 = _matchesRepo.GetMatchesByTournamentId(tournamentId).Where(m => m.Result == "Pending").ToList();//all remaining matches in tournament

            int teamspoints = 0;


            if (pt1 != null)
            {
                teamspoints = pt1.Points;
            }
            var pointstables = _pointsTableRepo.GetPointsTableByTournamentId(tournamentId).Include(p => p.Team).Where(p => p.TeamId != teamId).ToList();
            var matches = _matchesRepo.GetMatchesByTournamentId(tournamentId).Where(m => m.TeamAId != teamId && m.TeamBId != teamId && m.Result == "Pending").ToList();
            var teams = _teamsRepo.GetTeamsByTournamentId(tournamentId).Where(t => t.TeamId != teamId).ToList();

            //int remMatchPoints = (mt1.Count - matches.Count) * ppm;//without selected teams
            //int maxPoints = remMatchPoints + teamspoints;
            

            Console.WriteLine("matches :" + matches.Count);
            Console.WriteLine("all matches :" + mt1.Count);
            int remMatchPoints = matches.Count * ppm;//without selected teams
            int maxPoints = remMatchPoints + teamspoints;
            Console.WriteLine("remMatchPoints" + remMatchPoints);
            Console.WriteLine("teamspoints" + teamspoints);
            Console.WriteLine("maxPoints :" + maxPoints);

            Graph crickMaxFlowGraph = new Graph(maxPoints, t1, teams, matches, pointstables);
            crickMaxFlowGraph.DisplayCapacityMatrix();

            var maxFlowAlgorithm = new MaxFlowAlgorithm(crickMaxFlowGraph.GetGraph());

            int maxFlow = maxFlowAlgorithm.MaxFlow(0, crickMaxFlowGraph.GetSinkIndex());


            var graphMatrix = crickMaxFlowGraph.GetGraph();


            var message = "";
            var messagePart = "";
            Console.WriteLine($"MAX FLOW = > {maxFlow}");

            if (maxFlow == remMatchPoints/ppm)
            {
                messagePart = "Congratulations! ";
                message = $"Team {t1.Name} can reach the top of the points table.";
            }
            else
            {
                messagePart = "Unfortunately, ";
                message = $"Team {t1.Name} cannot reach the top of the points table.";
            }
            
            return RedirectToAction("Details", "Tournament", new { id = tournamentId, messagePart = messagePart, message = message });
            
        }
    }
}