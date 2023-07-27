using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<AppUser> objUserList = _db.AppUsers.ToList();
            return View(objUserList);
        }
    }
}
