using GymApp.Data;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public MealController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.GetUserID();
            var userMeals = _db.Meals.Where(r => r.User.Id == currentUser.ToString()).ToList();
            var mealViewModel = new MealViewModel()
            {
                Meals = userMeals
            };
            return View(mealViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Meal obj)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                obj.User = user;
                ModelState.Remove("User");
                ModelState.Remove("MealId");
            }
            obj.date = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Meals.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Meal");
            }
            else
            {
                foreach (var err in ModelState.Keys)
                {
                    var error = ModelState[err].Errors;
                    foreach(var errormessage in error)
                    {
                        string er = errormessage.ErrorMessage;
                    }
                }
            }

            return View();
        }
    }
}
