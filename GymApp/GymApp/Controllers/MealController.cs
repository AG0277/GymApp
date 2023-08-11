using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MealController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }
        public IActionResult Index()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userMeals = _db.Meals.Where(r => r.User.Id == currentUser.ToString());
            return View(userMeals.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Meal obj)
        {
            
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
