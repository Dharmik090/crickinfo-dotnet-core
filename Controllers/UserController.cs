using Microsoft.AspNetCore.Mvc;
using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interface;
using crickinfo_mvc_ef_core.Models.CreateModels;
using crickinfo_mvc_ef_core.ViewModels;
namespace crickinfo_mvc_ef_core.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _userRepo;
        private readonly ITournamentRepo _tournamentRepo;

        public UserController(IUserRepo userRepo, ITournamentRepo tournamentRepo)
        {
            _userRepo = userRepo;
            _tournamentRepo = tournamentRepo;
        }

        public ViewResult Index()
        {
            var model = _userRepo.GetUsers().FirstOrDefault();
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User model)
        {

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    IsAdmin = false
                };
                _userRepo.Add(user);

                HttpContext.Session.SetInt32("UserId", user.Id);

                return RedirectToAction("Details", new { id = user.Id });
            }

            return View(model);
        }



        [HttpGet]
        public ViewResult Update(int id)
        {
            User user = _userRepo.GetUserById(id);
            UserUpdateViewModel model = new UserUpdateViewModel() { Name = user.Name, Password = user.Password, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UserUpdateViewModel model)
        {

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };

                int? userId = HttpContext.Session.GetInt32("UserId");
                Console.WriteLine($"============================{user.Name}");
                _userRepo.Update(userId.Value,user);


                return RedirectToAction("Details", new { id = user.Id });
            }

            return View(model);
        }





        public ViewResult Details(int id)
        {
            User user = _userRepo.GetUserById(id);

            if (user == null)
            {
                Response.StatusCode = 404;
                return View("User Not Found", id);
            }

            IEnumerable<Tournament> tournaments = _tournamentRepo.GetTournamentByUserId(id);
            UserTournamentViewModel model = new UserTournamentViewModel() { User = user, Tournaments = tournaments };
            return View(model);
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin model)
        {

            if (ModelState.IsValid)
            {
                User user = _userRepo.GetUserByEmail(model.Email);

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        return RedirectToAction("Details", new { id = user.Id });
                    }

                    ViewBag.PasswordError = "Invalid Password";
                }

                ViewBag.EmailError = "Email does not exist";
            }
            return View(model);
        }

        public IActionResult Logout() {
            HttpContext.Session.Remove("UserId");
            // HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
