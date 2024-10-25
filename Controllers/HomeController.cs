using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace crickinfo_mvc_ef_core.Controllers
{
	public class HomeController : Controller
	{
		private readonly ITournamentRepo _tournamentRepo;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, ITournamentRepo tournamentRepo)
		{
			_logger = logger;
			_tournamentRepo = tournamentRepo;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
